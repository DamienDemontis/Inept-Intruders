using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerBehaviour : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI timerText;
    public enum TimerState
    {
        Running,
        Paused,
        Stopped,
        ReachingZero
    }

    [Header("Timer")]
    [SerializeField] private float _timer = 0;

    [SerializeField] private TimerState _timerState = TimerState.Stopped;

    private float _timerStart = 0;

    private void Start()
    {
        _timerStart = _timer;
    }

    void Update()
    {
        if (timerText == null)
        {
            Debug.LogWarning("Timer Text is not assigned in the inspector.");
            return;
        }

        float deltaTime = Time.deltaTime;

        if (_timerState == TimerState.Running)
        {

            _timer = Mathf.Max(_timer - deltaTime, 0);


            if (_timer <= 0)
            {
                _timer = 0;
                _timerState = TimerState.ReachingZero;
            }
            else
            {
                _timerState = TimerState.Running;
            }
        }

        int minutes = (int)_timer / 60;
        int seconds = (int)_timer % 60;

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public float Timer
    {
        get => _timer;
        set => _timer = value;
    }

    public TimerState TimerStatus
    {
        get => _timerState;
    }

    public void StartTimer()
    {
        _timerState = TimerState.Running;
    }

    public void PauseTimer()
    {
        _timerState = TimerState.Paused;
    }

    public void StopTimer()
    {
        _timerState = TimerState.Stopped;
    }

    public void ResetTimer()
    {
        _timer = _timerStart;
        _timerState = TimerState.Stopped;
    }
}
