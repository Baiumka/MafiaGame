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
    public VoidHandler onBackPressed;
    public GameStateHandler onGameStateChanged;
    public LeftSecondsHandler onTimerTicked;
    public DayHandler onCameNewDay;
    public PlayerHandler onNewSpeaker;
    public PlayerHandler onPlayerVotedTurn;
    public VoteOfficialHandler onVoteOfficial;
    public IntHandler onVotesChanged;

    private Player playerSlotSelecting;
    private Player playerRoleSelecting;

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

    public void NextState()
    {
        gameManager.NextState();
    }

    private void Start()
    {
        if(gameManager == null)
        {
            gameManager = new GameManager();
            gameManager.onGameManagerGotError += ShowError;
            gameManager.onGameStarted += StartGame;
            gameManager.onTimerTicked += TickTimer;
            gameManager.onGameStateChanged += StateChange;
            gameManager.onCameNewDay += NewDay;
            gameManager.onNewSpeaker += ChangeSpeaker;
            gameManager.onVoteOfficial += VoteOfficial;
            gameManager.onPlayerVotedTurn += VotedTurn;
            gameManager.onVotesChanged += UpdateVotes;
        }
    }

    

    public void ResetTime()
    {
        gameManager.ResetTimer();
    }

    public void BonusTime(int seconds)
    {
        gameManager.BonusTime(seconds);
    }

    public void PutToVote(Player voted)
    {
        gameManager.PutToVote(voted);
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
                if (playerSlotSelecting != null)
                {
                    onWantedGameWindow?.Invoke();
                    playerSlotSelecting = null;
                }
                else
                {
                    onBackPressed?.Invoke();
                    if (playerRoleSelecting != null)
                    {
                        playerRoleSelecting = null;
                    }
                }
                break;
        }
    }

    public void PrepareForRole(Player player)
    {
        playerRoleSelecting = player;
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

    public void SelectRole(Role role)
    {
        if (playerRoleSelecting != null)
        {
            playerRoleSelecting.SetRole(role);
            playerRoleSelecting = null;
            onBackPressed?.Invoke();
        }
        else
        {
            onError?.Invoke("ERROR_NO_PLAYER_FOR_ROLE");
        }
    }

    public void SelectPeopleForSlot(Player player)
    {
        if (gameManager.GameState == GameState.GIVE_ROLE)
        {
            onWantedPlayerList?.Invoke();
            playerSlotSelecting = player;
        }
        else if(gameManager.GameState == GameState.VOTE)
        {
            if (player.Voted == null)
            {
                gameManager.Vote(player);
            }
            else
            {
                gameManager.TryUnVote(player);
            }
        }
    }

    public void ShowPlayerList()
    {
        onWantedPlayerList?.Invoke();
    }

    public void CreateGame(int playerCount)
    {
        gameManager.CreateGame(playerCount);
    }

    public void StartGame()
    {
        gameManager.StartGame();
    }

    #region Events Duplicates

    private void StateChange(GameState gameState)
    {
        onGameStateChanged?.Invoke(gameState);
    }
    private void VotedTurn(Player player)
    {
        onPlayerVotedTurn?.Invoke(player);
    }

    private void TickTimer(int now, int final)
    {
        onTimerTicked?.Invoke(now, final);
    }
    private void ShowError(string errorText)
    {
        onError?.Invoke(errorText);
    }

    private void UpdateVotes(int i)
    {
        onVotesChanged?.Invoke(i);
    }
    private void StartGame(Game newGame)
    {
        onGameStarted?.Invoke(newGame);
    }

    private void ChangeSpeaker(Player player)
    {
        onNewSpeaker?.Invoke(player);
    }

    private void NewDay(int dayNumber)
    {
        onCameNewDay?.Invoke(dayNumber);
    }
    private void VoteOfficial(List<Player> votedPlayers)
    {
        onVoteOfficial?.Invoke(votedPlayers);
    }
    #endregion
}
