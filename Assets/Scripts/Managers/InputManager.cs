using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public DefaultControls controls {  get; private set; }

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;

        controls = new DefaultControls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}