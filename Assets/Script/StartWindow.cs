using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartWindow : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] TMP_InputField playerCountField;
    [SerializeField] Button playersButton;
    [SerializeField] Button loginButton;
    [SerializeField] TMP_InputField loginInput;
    [SerializeField] TMP_InputField passwordInput;
    
    void Start()
    {
        startButton.onClick.AddListener(onStartClick);   
        playersButton.onClick.AddListener(onPlayerClick);
        loginButton.onClick.AddListener(onLoginClick);
    }

    private void onLoginClick()
    {
        Controller.singlton.Login(loginInput.text, passwordInput.text);
    }

    private void onPlayerClick()
    {
        Controller.singlton.ShowPlayerList();
    }

    private void onStartClick()
    {
        Controller.singlton.CreateGame(10);
    }

}
