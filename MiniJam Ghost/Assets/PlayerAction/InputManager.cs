using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    private static PlayerAction controls;
    public static PlayerAction Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new PlayerAction();
        }
    }

    private void Awake()
    {
        if (controls != null) { return; }
        controls = new PlayerAction();
    }

    private void OnEnable() => Controls.Enable();
    private void OnDisable() => Controls.Disable();
    private void OnDestroy() => controls = null;


}
