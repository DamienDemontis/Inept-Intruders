using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private RobertInteractionController _robertInteractionController;
    public RobertInteractionController RobertInteractionController => _robertInteractionController;

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
        _robertInteractionController = GameObject.FindGameObjectWithTag("RobertPlayer").GetComponent<RobertInteractionController>();
    }
}
