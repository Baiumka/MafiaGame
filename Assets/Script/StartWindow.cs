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
    [SerializeField] TMP_Text userNumberText;
    [SerializeField] GameObject loginPanel;
    
    void Start()
    {
        loginPanel.SetActive(true);
        userNumberText.gameObject.SetActive(false);

        startButton.onClick.AddListener(onStartClick);   
        playersButton.onClick.AddListener(onPlayerClick);
        loginButton.onClick.AddListener(onLoginClick);

        Controller.singlton.onUserLogin += ShowUserInfo;
    }

    private void ShowUserInfo(int number)
    {
        loginPanel.SetActive(false);
        userNumberText.gameObject.SetActive(true);
        userNumberText.text = number.ToString();
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
