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

        Debug.Log("Checking");
        if (Input.anyKeyDown)
        {
            Debug.Log("Any Key Down");  
            if (Input.GetKeyDown(KeyCode.Joystick1Button0)) { Debug.Log("0"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button1)) { Debug.Log("1"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button2)) { Debug.Log("2"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button3)) { Debug.Log("3"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button4)) { Debug.Log("4"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button5)) { Debug.Log("5"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button6)) { Debug.Log("6"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button7)) { Debug.Log("7"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button8)) { Debug.Log("8"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button9)) { Debug.Log("9"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button10)) { Debug.Log("10"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button11)) { Debug.Log("11"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button12)) { Debug.Log("12"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button13)) { Debug.Log("13"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button14)) { Debug.Log("14"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button15)) { Debug.Log("15"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button16)) { Debug.Log("16"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button17)) { Debug.Log("17"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button18)) { Debug.Log("18"); return InputMode.Controller; }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button19)) { Debug.Log("19"); return InputMode.Controller; }
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
