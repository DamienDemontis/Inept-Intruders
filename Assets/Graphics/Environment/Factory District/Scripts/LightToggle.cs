//Coded for Unity 5.6 or higher

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightToggle : MonoBehaviour
{
    [Header("Light Settings")]
    [Tooltip("Toggles The Source Light On/Off")]
    public bool ToggleLight;
    [Tooltip("The Objects Light Source")]
    public GameObject LightSource;


    void Start()
    {
        if (ToggleLight)
        {
            LightSource.SetActive(true);
        }

        if (!ToggleLight)
        {
            LightSource.SetActive(false);
        }

    }
    void Update()
    {
        if (ToggleLight)
        {
            LightSource.SetActive(true);
        }

        if (!ToggleLight)
        {
            LightSource.SetActive(false);
        }
    }
}
