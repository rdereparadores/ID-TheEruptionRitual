using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRKeyboardHandler : MonoBehaviour, IPointerDownHandler
{
    private TMP_InputField _input;
    private TouchScreenKeyboard _keyboard;

    private void Start()
    {
        _input = GetComponent<TMP_InputField>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("doiasjodjasodja");
        #if UNITY_ANDROID && !UNITY_EDITOR
            OpenKeyboard();
        #else
            _input.ActivateInputField();
        #endif
    }

    private void OpenKeyboard()
    {
        _keyboard = TouchScreenKeyboard.Open(_input.text, TouchScreenKeyboardType.Default);
    }

    private void Update()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            if (_keyboard == null) return;
        
            if (_keyboard.status == TouchScreenKeyboard.Status.Visible)
            {
            _input.text = _keyboard.text;
            }
        #endif
    }
}
