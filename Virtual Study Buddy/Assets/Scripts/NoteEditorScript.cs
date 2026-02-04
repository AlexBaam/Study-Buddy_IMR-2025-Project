using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using System;

public class StickyNoteTextEditor : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public NonNativeKeyboard keyboard;
    public TMP_InputField keyboardInputField;
    public InputActionReference leftTrigger;

    bool triggerWasPressed;

    void OnEnable()
    {
        leftTrigger?.action.Enable();
    }

    void OnDisable()
    {
        leftTrigger?.action.Disable();
    }

    void Update()
    {
        bool keyPressed = Input.GetKeyDown(KeyCode.E);

        bool triggerPressed = false;
        if (leftTrigger != null)
        {
            float v = leftTrigger.action.ReadValue<float>();
            bool pressedNow = v > 0.1f;
            triggerPressed = pressedNow && !triggerWasPressed;
            triggerWasPressed = pressedNow;
        }

        if (keyPressed || triggerPressed)
        {
            OpenKeyboard();
        }
    }

    void OpenKeyboard()
    {
        if (keyboard == null || keyboardInputField == null)
            return;

        keyboard.gameObject.SetActive(true);
        keyboardInputField.text = textField.text;

        keyboard.OnTextSubmitted += HandleTextSubmitted;
        keyboard.OnClosed += HandleKeyboardClosed;
    }

    void HandleTextSubmitted(object sender, EventArgs e)
    {
        textField.text = keyboardInputField.text;
    }

    void HandleKeyboardClosed(object sender, EventArgs e)
    {
        keyboard.OnTextSubmitted -= HandleTextSubmitted;
        keyboard.OnClosed -= HandleKeyboardClosed;
    }
}