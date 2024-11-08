using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void VoidHandler();
public class Controller : MonoBehaviour
{
    public static Controller singlton;

    private GameManager gameManager;
    
    public StartGameHandler onGameStarted;
    public ErrorHandler onError;
    public VoidHandler onWantedPlayerList;
    public VoidHandler onWantedStartWidnow;
    public VoidHandler onWantedGameWindow;

    private Player playerSlotSelecting;

    private void Awake()
    {    
        if (singlton == null)
        {
            singlton = this;
        }
        else
        {
            Destroy(this);
        }        
    }

    private void Start()
    {
        if(gameManager == null)
        {
            gameManager = new GameManager();
            gameManager.onGameManagerGotError += ShowError;
            gameManager.onGameStarted += StartGame;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnBack();            
        }
    }

    private void ReturnBack()
    {
        switch (gameManager.GameState)
        {
            case GameState.NONE:
                onWantedStartWidnow?.Invoke();
                playerSlotSelecting = null;
                break;
            case GameState.GIVE_ROLE:
                onWantedGameWindow?.Invoke();
                playerSlotSelecting = null;
                break;
        }
    }

    public void PeopleClick(People peopleInfo)
    {
        if(gameManager.GameState == GameState.GIVE_ROLE)
        {
            if(playerSlotSelecting != null)
            {
                playerSlotSelecting.SetPeople(peopleInfo);
                ReturnBack();

            }
        }
        else
        {
            Debug.Log(peopleInfo.Nickname);
        }
    }


    public void SelectPeopleForSlot(Player player)
    {
        onWantedPlayerList?.Invoke();
        playerSlotSelecting = player;
    }

    public void ShowPlayerList()
    {
        onWantedPlayerList?.Invoke();
    }

    public void StartNewGame(int playerCount)
    {
        gameManager.StartGame(playerCount);
    }

    #region Events Duplicates
    private void ShowError(string errorText)
    {
        onError?.Invoke(errorText);
    }

    private void StartGame(Game newGame)
    {
        onGameStarted?.Invoke(newGame);
    }
    #endregion
}
