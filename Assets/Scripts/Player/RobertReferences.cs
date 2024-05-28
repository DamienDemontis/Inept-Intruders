using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobertReferences : MonoBehaviour
{
    [SerializeField] private RobertMovementController robertMovementController;
    [SerializeField] private RobertInteractionController robertInteractionController;
    [SerializeField] private RobertCameraController robertCameraController;

    public RobertMovementController RobertMovementController => robertMovementController;
    public RobertInteractionController RobertInteractionController => robertInteractionController;
    public RobertCameraController RobertCameraController => robertCameraController;
}
