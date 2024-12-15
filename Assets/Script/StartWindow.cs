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
    [SerializeField] Button regButton;
    [SerializeField] Button finalRegButton;
    [SerializeField] Button backButton;
    [SerializeField] Button gamesListButton;
    [SerializeField] TMP_InputField loginInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_Text userNumberText;
    [SerializeField] TMP_Text userNameText;

    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject regPanel;
    [SerializeField] GameObject gamePanel;
    
    
    public void Init()
    {
        loginPanel.SetActive(true);
        gamePanel.SetActive(false);
        regPanel.SetActive(false);
        

        startButton.onClick.AddListener(onStartClick);
        playersButton.onClick.AddListener(onPlayerClick);
        loginButton.onClick.AddListener(onLoginClick);
        gamesListButton.onClick.AddListener(onGamesListClick);
        regButton.onClick.AddListener(onRegButton);
        backButton.onClick.AddListener(onBackButton);
        finalRegButton.onClick.AddListener(onFinalRegClick);

        Controller.singlton.onUserLogin += ShowUserInfo;
    }

    private void onGamesListClick()
    {
        Controller.singlton.ShowGamesList();
    }

    private void onFinalRegClick()
    {
        Controller.singlton.Register(loginInput.text, passwordInput.text, nameInput.text);
    }

    private void onBackButton()
    {
        loginPanel.SetActive(true);
        gamePanel.SetActive(false);
        regPanel.SetActive(false);
    }

    private void onRegButton()
    {
        loginPanel.SetActive(false);
        gamePanel.SetActive(false);
        regPanel.SetActive(true);
    }

    private void ShowUserInfo(int number, string name)
    {
        loginPanel.SetActive(false);
        gamePanel.SetActive(true);
        regPanel.SetActive(false);


        userNumberText.text = number.ToString();
        userNameText.text = name.ToString();
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
        int players = Convert.ToInt32(playerCountField.text);
        Controller.singlton.CreateGame(players);
    }

}
