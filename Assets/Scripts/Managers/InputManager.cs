using System;
using UnityEngine;

public enum InputMode { Keyboard, Controller }
public class InputManager : MonoBehaviour
{
    public InputMode currentMode;
    public InputMode currentModeLastFrame;
    public static Action<InputMode> OnInputModeChanged;

    void Start()
    {
        currentMode = InputMode.Keyboard;
    }

    // Update is called once per frame
    void Update()
    {
        currentMode = ProcessInputMode();

        if (currentMode != InputMode.Keyboard)
        {
            OnInputModeChanged?.Invoke(currentMode);
        }

        currentModeLastFrame = ProcessInputMode();
    }

    private InputMode ProcessInputMode()
    {
        if (Input.GetJoystickNames().Length == 0)
        { return InputMode.Keyboard; }

        if (Input.anyKeyDown)
        {
            if ((Input.GetKeyDown(KeyCode.Joystick1Button0))
            || (Input.GetKeyDown(KeyCode.Joystick1Button1))
            || (Input.GetKeyDown(KeyCode.Joystick1Button2))
            || (Input.GetKeyDown(KeyCode.Joystick1Button3))
            || (Input.GetKeyDown(KeyCode.Joystick1Button4))
            || (Input.GetKeyDown(KeyCode.Joystick1Button5))
            || (Input.GetKeyDown(KeyCode.Joystick1Button6))
            || (Input.GetKeyDown(KeyCode.Joystick1Button7))
            || (Input.GetKeyDown(KeyCode.Joystick1Button8))
            || (Input.GetKeyDown(KeyCode.Joystick1Button9))
            || (Input.GetKeyDown(KeyCode.Joystick1Button10))
            || (Input.GetKeyDown(KeyCode.Joystick1Button11))
            || (Input.GetKeyDown(KeyCode.Joystick1Button12))
            || (Input.GetKeyDown(KeyCode.Joystick1Button13))
            || (Input.GetKeyDown(KeyCode.Joystick1Button14))
            || (Input.GetKeyDown(KeyCode.Joystick1Button15))
            || (Input.GetKeyDown(KeyCode.Joystick1Button16))
            || (Input.GetKeyDown(KeyCode.Joystick1Button17))
            || (Input.GetKeyDown(KeyCode.Joystick1Button18))
            || (Input.GetKeyDown(KeyCode.Joystick1Button19)))
            { return InputMode.Controller; }
            else
            { return InputMode.Keyboard; }
        }

        if (Input.anyKey)
        {
            if (Input.GetAxisRaw("Horizontal") != 0) return InputMode.Keyboard;
            if (Input.GetAxisRaw("Vertical") != 0) return InputMode.Keyboard;
        }

        if (Input.GetAxisRaw("Horizontal") != 0) return InputMode.Controller;
        if (Input.GetAxisRaw("Vertical") != 0) return InputMode.Controller;

        if (Input.GetAxisRaw("Horizontal2") != 0) return InputMode.Controller;
        if (Input.GetAxisRaw("Vertical2") != 0) return InputMode.Controller;

        if (Input.GetAxisRaw("HorizontalD") != 0) return InputMode.Controller;
        if (Input.GetAxisRaw("VerticalD") != 0) return InputMode.Controller;

        if (Input.GetAxisRaw("LeftTrigger") < 0) return InputMode.Controller;
        if (Input.GetAxisRaw("RightTrigger") < 0) return InputMode.Controller;

        return currentMode;
    }
}
