using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class GameWindow : MonoBehaviour
{
    [SerializeField] Transform playerPanel;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject roleContextMenu;
    [SerializeField] Button startGameButton;
    [SerializeField] TickPanel tickPanel;
    [SerializeField] TMP_Text dayText;
    

    private List<PlayerObject> playersList;
    private GameState visibleGameState = GameState.GIVE_ROLE;

    private void Awake()
    {
        roleContextMenu.SetActive(false);
        startGameButton.gameObject.SetActive(true);
        tickPanel.gameObject.SetActive(false);
    }

    public void Init()
    {
        Controller.singlton.onWantedStartWidnow += HideContextMenu;
        Controller.singlton.onBackPressed += HideContextMenu;
        Controller.singlton.onGameStateChanged += ChangeState;
        Controller.singlton.onTimerTicked += TickTimer;
        Controller.singlton.onCameNewDay += NewDay;
        Controller.singlton.onNewSpeaker += ChangeSpeaker;
        Controller.singlton.onVoteOfficial += VoteOfficial;
        Controller.singlton.onPlayerVotedTurn += VoteTurn;
        Controller.singlton.onVotesChanged += UpdateVotes;
        Controller.singlton.onDopSpeakOfficial += DopSpeakOfficial;
        Controller.singlton.onDopSpeakStarted += DopSpeakStart;
        Controller.singlton.onDopVoteOfficial += DopVoteOfficial;
       // Controller.singlton.onNightStarted += StartNight;
        Controller.singlton.onLastWordStarted += LastWord;
        

        startGameButton.onClick.AddListener(OnStartButtonClick);
        tickPanel.Init();
    }

   

    private void LastWord(Player player)
    {
        tickPanel.LastWord(player);
    }

  

    private void DopVoteOfficial(List<Player> votedPlayers)
    {
        ChangeState(GameState.DOP_VOTE_OFFICIAL);
        tickPanel.DopVoteOfficial(votedPlayers);
    }

    private void DopSpeakStart(Player player)
    {
        ChangeState(GameState.DOP_SPEAK);
        ChangeSpeaker(player);
    }

    private void DopSpeakOfficial(List<Player> votedPlayers)
    {
        ChangeState(GameState.DOP_SPEAK);
        tickPanel.DopSpeakOfficial(votedPlayers);
    }

    private void UpdateVotes(int i)
    {
        tickPanel.SetVoteCount(i);
    }

    private void VoteTurn(Player player)
    {
        ChangeState(GameState.VOTE);
        tickPanel.VotePlayer(player);
    }

    private void VoteOfficial(List<Player> votedPlayers)
    {
        ChangeState(GameState.VOTE_OFFIACIAL);
        tickPanel.Clean();
        tickPanel.VoteOfficial(votedPlayers);
    }

    private void ChangeSpeaker(Player player)
    {
        tickPanel.SetSpeaker(player);
    }

    private void NewDay(int dayNumber)
    {
        
        dayText.SetText(Translator.Message(Messages.DAY) + dayNumber);
    }

    private void TickTimer(int now, int final)
    {        
        tickPanel.SetSeconds(final - now);
    }

    private void ChangeState(GameState gameState)
    {
        if (visibleGameState == gameState) return;   
        visibleGameState = gameState;
        switch(visibleGameState)
        {
            case GameState.BEST_TURN:
                tickPanel.SetState(visibleGameState);
                break;
            case GameState.SHERIF:
                tickPanel.SetState(visibleGameState);
                break;
            case GameState.BOSS:
                tickPanel.SetState(visibleGameState);                
                break;
            case GameState.SHOOTING:
                foreach (PlayerObject po in playersList)
                {
                    po.ShowRole();
                }
                tickPanel.SetState(visibleGameState);
                break;
            case GameState.NIGHT:                
                dayText.text = Translator.Message(Messages.NIGHT);
                tickPanel.SetState(visibleGameState);
                break;
            case GameState.VOTE_FOR_UP:
                tickPanel.VoteForUp();
                break;
            case GameState.DOP_SPEAK:
                dayText.text = Translator.Message(Messages.CROSSFIRE);
                break;
            case GameState.VOTE:
                dayText.text = Translator.Message(Messages.COURT);
                foreach (PlayerObject po in playersList)
                {
                    po.HideVoteButton();
                }
                break;
            case GameState.VOTE_OFFIACIAL:
                dayText.text = Translator.Message(Messages.COURT);
                foreach (PlayerObject po in playersList)
                {
                    po.HideVoteButton();
                }
                break;
            case GameState.DAY:
                tickPanel.gameObject.SetActive(true);
                tickPanel.SetState(visibleGameState);
                foreach(PlayerObject po in playersList)
                {
                    po.ShowVoteButton();
                }
                break;
            case GameState.MORNING:
                foreach (PlayerObject po in playersList)
                {
                    po.HideRole();
                }
                dayText.SetText(Translator.Message(Messages.MORNING));
                tickPanel.Clean();
                break;
            case GameState.FIRST_NIGHT_MAFIA:
                startGameButton.gameObject.SetActive(false);
                tickPanel.gameObject.SetActive(true);
                tickPanel.SetState(visibleGameState);
                dayText.SetText(Translator.Message(Messages.FIRST_NIGHT));
                foreach (PlayerObject po in playersList)
                {
                    po.BlockRoleButton();
                }
                break;
            case GameState.FIRST_NIGHT_SHERIF:
                tickPanel.SetState(visibleGameState);              
                break;
            default:
                break;
        }
    }

    private void OnStartButtonClick()
    {
        Controller.singlton.StartGame();
    }

    private void HideContextMenu()
    {
        roleContextMenu.SetActive(false);
    }

    public void InitGame(Game game)
    {
        playersList = new List<PlayerObject>(game.Players.Count);
        foreach(Transform child in playerPanel) Destroy(child.gameObject);
        foreach(Player player in game.Players)
        {
            GameObject newPlayerObject = GameObject.Instantiate(playerPrefab, playerPanel);
            PlayerObject po = newPlayerObject.GetComponent<PlayerObject>();
            po.Init(player);
            po.onRoleClicked += ShowRoleContext;
            playersList.Add(po);    


        }
    }

    private void ShowRoleContext(Vector3 pos, Player player)
    {
        Controller.singlton.PrepareForRole(player);        
        if (pos.y > 855)
            pos = new Vector3(pos.x,855,pos.z);
        roleContextMenu.transform.position = pos;
        roleContextMenu.SetActive(true);
    }

    private void OnDestroy()
    {
        Controller.singlton.onBackPressed -= HideContextMenu;
    }
}
