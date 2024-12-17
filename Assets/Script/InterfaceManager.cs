using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] GameWindow gameWindow;
    [SerializeField] FinalWindow finalWindow;
    [SerializeField] StartWindow startWindow;
    [SerializeField] SelectPlayerWindow selectPlayerWindow;
    [SerializeField] DialogWindow dialogWindow;
    [SerializeField] GameListWindow gameListWindow;

    public static DialogWindow dialog;
    private void Awake()
    {
        if (dialog == null)
        {
            dialog = dialogWindow;
        }
        else
        {
            Destroy(dialog);
        }       
    }

    public void Init()//Вместо Start()
    {
        Controller.singlton.onError += ShowError;
        Controller.singlton.onGameStarted += StartGame;
        Controller.singlton.onWantedPlayerList += ShowSelectPlayer;
        Controller.singlton.onWantedGamesList += ShowGameList;
        Controller.singlton.onWantedGameWindow += ShowGameWindow;
        Controller.singlton.onWantedStartWidnow += ShowStartWindow;
        Controller.singlton.onGameDetailsUpdated += ShowGameDetailsWindow;
        Controller.singlton.onBackPressed += DoBack;

        InitOrder();
        startWindow.Init();
        gameListWindow.Init();
        finalWindow.Init();
        selectPlayerWindow.Init();
    }

    private void DoBack()
    {
        if (gameListWindow.gameObject.activeSelf)
        {
            ShowWindow(startWindow);
            return;
        }

        if (selectPlayerWindow.gameObject.activeSelf)
        {
            ShowWindow(startWindow);
            return;
        }

        if (finalWindow.gameObject.activeSelf) 
        {
            ShowWindow(gameListWindow);
            return;
        }
       
    }

    private void ShowGameDetailsWindow(List<ResultPlayer> players, List<ResultHistoryEvent> history, GameInfo game)
    {
        finalWindow.DrawResults(players, history, game);
        ShowWindow(finalWindow);
    }

    private void ShowGameList()
    {
        ShowWindow(gameListWindow);
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
        selectPlayerWindow.DrawList();
        ShowWindow(selectPlayerWindow);
    }

    private void StartGame(Game game)
    {
        ShowWindow(gameWindow);

        gameWindow.InitGame(game);
    }

    private void ShowError(string errorText)
    {
        dialog.ShowDialog(
            errorText,
            null,
            null
            );
        Debug.Log(errorText);
    }

    private void InitOrder()
    {
        gameWindow.gameObject.SetActive(false);
        finalWindow.gameObject.SetActive(false);
        selectPlayerWindow.gameObject.SetActive(false);
        startWindow.gameObject.SetActive(true);
        gameListWindow.gameObject.SetActive(false);

        dialogWindow.gameObject.SetActive(false);
    }

    private void ShowWindow(MonoBehaviour window)
    {
        gameWindow.gameObject.SetActive(false);
        selectPlayerWindow.gameObject.SetActive(false);
        startWindow.gameObject.SetActive(false);
        finalWindow.gameObject.SetActive(false);
        gameListWindow.gameObject.SetActive(false);

        window.gameObject.SetActive(true);
    }


}
