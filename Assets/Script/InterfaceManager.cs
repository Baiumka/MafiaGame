using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] GameWindow gameWindow;
    [SerializeField] StartWindow startWindow;
    [SerializeField] SelectPlayerWindow selectPlayerWindow;
    //[SerializeField] DialogWindow dialog;
    private void Start()
    {
        Controller.singlton.onError += ShowError;
        Controller.singlton.onGameStarted += StartGame;
        Controller.singlton.onWantedPlayerList += ShowSelectPlayer;
        Controller.singlton.onWantedGameWindow += ShowGameWindow;
        Controller.singlton.onWantedStartWidnow += ShowStartWindow;
        InitOrder();
    }

    private void ShowGameWindow()
    {
        ShowWindow(gameWindow);
    }

    private void ShowStartWindow()
    {
        ShowWindow(startWindow);
    }

    private void ShowSelectPlayer()
    {
        selectPlayerWindow.DrawPeople();
        ShowWindow(selectPlayerWindow);
    }

    private void StartGame(Game game)
    {
        ShowWindow(gameWindow);

        gameWindow.InitGame(game);
    }

    private void ShowError(string errorText)
    {
        Debug.Log(errorText);
    }

    private void InitOrder()
    {
        gameWindow.gameObject.SetActive(false);
        selectPlayerWindow.gameObject.SetActive(false);
        startWindow.gameObject.SetActive(true);
    }

    private void ShowWindow(MonoBehaviour window)
    {
        gameWindow.gameObject.SetActive(false);
        selectPlayerWindow.gameObject.SetActive(false);
        startWindow.gameObject.SetActive(false);

        window.gameObject.SetActive(true);
    }
}
