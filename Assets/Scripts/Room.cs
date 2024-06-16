using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeReference] private List<IInteractable> _interactablesList = new List<IInteractable>();
    [SerializeReference] private List<SurveillanceCamera> _surveillanceCamerasList = new List<SurveillanceCamera>();
    [SerializeField] private string _id;

    void Start()
    {
        GameObject[] boardInteractableList = GameObject.FindGameObjectsWithTag("BoardInteractable");

        foreach (GameObject boardInteractible in boardInteractableList)
        {
            IInteractable interactable = boardInteractible.GetComponentInChildren<IInteractable>();

            if (interactable != null)
            {
                Debug.Log($"[Room::Start] Added interactable for room");
                _interactablesList.Add(interactable);
            }
        }

        foreach (SurveillanceCamera surveillanceCamera in _surveillanceCamerasList)
        {
            surveillanceCamera.Deactivate();
        }
    }

    public List<IInteractable> InteractablesList
    { get { return _interactablesList; } }

    public List<SurveillanceCamera> SurveillanceCamerasList
    { get { return _surveillanceCamerasList; } }
}