using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour, IInteractable
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField, Tooltip("Wait time at each waypoint before moving to the next one")] private float pauseTime = 0;
    [Space]
    [SerializeField] private List<Transform> waypoints;

    [Header("Interactions")]
    [SerializeField] private Vector3 interactionSize = Vector3.one;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Modifiers")]
    [SerializeField, Tooltip("Objects that touch the platform follow its movement")] private bool makeFollower = false;
    [SerializeField, Tooltip("Go back and forth between waypoints, instead of looping")] private bool makeRewinder = false;
    [SerializeField, Tooltip("Only start moving once the player is detected on the platform")] private bool shouldWaitPlayer = false;

    private int _currentWaypointIndex = 0;
    private float _currentPauseTime = 0;

    private bool _isRewinding = false;
    private bool _canMove = false;

    private List<GameObject> _followerObjects = new List<GameObject>();

    private void Start()
    {
        if (shouldWaitPlayer)
        {
            _canMove = false;
        }
    }

    private void Update()
    {
        if (makeFollower)
        {
            CheckFollowerObjects();
        }

        if (!_canMove)
        {
            return;
        }

        if (Vector3.Distance(waypoints[_currentWaypointIndex].position, transform.position) < 0.1f)
        {
            GoToNextWaypoint();
        }

        if (_currentPauseTime > 0)
        {
            _currentPauseTime -= Time.deltaTime;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[_currentWaypointIndex].position, Time.deltaTime * moveSpeed);
    }

    private void GoToNextWaypoint()
    {
        _currentPauseTime = pauseTime;

        if (makeRewinder && _isRewinding)
        {
            _currentWaypointIndex--;
        }
        else
        {
            _currentWaypointIndex++;
        }

        if (_currentWaypointIndex >= waypoints.Count)
        {
            if (makeRewinder)
            {
                _currentWaypointIndex--;
                _isRewinding = true;
            }
            else
            {
                _currentWaypointIndex = 0;
            }
        }
        else if (_currentWaypointIndex <= 0 && makeRewinder)
        {
            _currentWaypointIndex = 0;
            _isRewinding = false;
        }
    }

    private void CheckFollowerObjects()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, interactionSize, Quaternion.identity, interactableLayer);

        // check if follower object is still touching platform, if not, remove from followers
        foreach (GameObject followerObject in _followerObjects)
        {
            bool foundObject = false;

            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject == followerObject)
                {
                    foundObject = true;
                    break;
                }
            }

            if (!foundObject)
            {
                if (followerObject && followerObject.transform.parent == transform)
                {
                    followerObject.transform.SetParent(null);
                }
                _followerObjects.Remove(followerObject);
                break;
            }
        }

        // add all colliding objects as followers
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (!_followerObjects.Contains(hitColliders[i].gameObject))
            {
                _followerObjects.Add(hitColliders[i].gameObject);
                hitColliders[i].transform.SetParent(transform);

                if (shouldWaitPlayer && hitColliders[i].gameObject.tag == "RobertPlayer")
                {
                    _canMove = true;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, interactionSize);
    }

    public void Interact()
    {
        _canMove = !_canMove;
    }

    public string GetId()
    {
        return string.Empty;
    }
}
