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

        Controller.singlton.onMafiaWin += MafiaWin;
        Controller.singlton.onCitizenWin += CitizenWin;
        Controller.singlton.onNoWin += NoWin;
        InitOrder();
        startWindow.Init();
        gameListWindow.Init();
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

    private void NoWin(Game game)
    {
        finalWindow.DrawNoWin(game);
        ShowWindow(finalWindow);
    }

    private void CitizenWin(Game game)
    {
        finalWindow.DrawCitizenWin(game);
        ShowWindow(finalWindow);
    }

    private void MafiaWin(Game game)
    {
        finalWindow.DrawMafiaWin(game);
        ShowWindow(finalWindow);
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
