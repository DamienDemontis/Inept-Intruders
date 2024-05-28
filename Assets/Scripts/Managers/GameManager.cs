using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int roomIndex = 0;
    [SerializeField] private List<CheckpointTrigger> checkpoints;

    [Header("Fade Effect")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeInTime = 1.5f;
    [SerializeField] private float fadeOutTime = 0.5f;

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private RobertReferences _robertReferences;
    public RobertReferences RobertReferences => _robertReferences;

    private int _lastCheckpoint = 0;
    public int LastCheckpoint => _lastCheckpoint;

    private void Awake()
    {
        if (_instance)
        {
            Destroy(this);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        StartCoroutine(FadeIn());

        GameObject robertPlayerObject = GameObject.FindGameObjectWithTag("RobertPlayer");
        _robertReferences = robertPlayerObject.GetComponent<RobertReferences>();

        if (PlayerPrefs.HasKey("roomIndex") && PlayerPrefs.GetInt("roomIndex") == roomIndex)
        {
            LoadLastCheckpoint();
        } else
        {
            PlayerPrefs.SetInt("roomIndex", roomIndex);
            SaveLastCheckpoint(0);

            LoadLastCheckpoint();
        }
    }

    private IEnumerator FadeIn()
    {
        fadeCanvasGroup.alpha = 1.0f;
        fadeCanvasGroup.DOFade(0.0f, fadeInTime);

        InputManager.Instance.InputControls.Disable();

        yield return new WaitForSeconds(fadeInTime);

        InputManager.Instance.InputControls.Enable();
    }

    public void SaveLastCheckpoint(int checkpointIndex)
    {
        _lastCheckpoint = checkpointIndex;
        PlayerPrefs.SetInt("roomCheckpoint", _lastCheckpoint);
    }

    private void LoadLastCheckpoint()
    {
        _lastCheckpoint = PlayerPrefs.GetInt("roomCheckpoint");
        Transform saveTransform = checkpoints[_lastCheckpoint].SaveTransform;

        _robertReferences.RobertMovementController.ForcePosition(saveTransform.position);
        _robertReferences.RobertCameraController.ForceYRotation(saveTransform.rotation.eulerAngles.y);
    }

    public void TriggerRobertPlayerDeath()
    {
        StartCoroutine(ChangeScene(SceneManager.GetActiveScene().name));
    }

    public IEnumerator ChangeScene(string sceneName)
    {
        fadeCanvasGroup.alpha = 0.0f;
        fadeCanvasGroup.DOFade(1.0f, fadeOutTime);

        InputManager.Instance.InputControls.Disable();

        yield return new WaitForSeconds(fadeOutTime);

        SceneManager.LoadScene(sceneName);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}
