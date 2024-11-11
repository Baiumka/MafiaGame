using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWindow : MonoBehaviour
{
    [SerializeField] Transform playerPanel;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject roleContextMenu;
    [SerializeField] Button startGameButton;
    [SerializeField] TimerPanel timerPanel;

    private List<PlayerObject> playersList;
    private GameState visibleGameState = GameState.GIVE_ROLE;

    private void Awake()
    {
        roleContextMenu.SetActive(false);
        startGameButton.gameObject.SetActive(true);
        timerPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        Controller.singlton.onBackPressed += HideContextMenu;
        Controller.singlton.onGameStateChanged += ChangeState;
        Controller.singlton.onTimerTicked += TickTimer;


        startGameButton.onClick.AddListener(OnStartButtonClick);
    }

    private void TickTimer(int now, int final)
    {
        timerPanel.SetSeconds(final - now);
    }

    private void ChangeState(GameState gameState)
    {
        visibleGameState = gameState;

        switch(visibleGameState)
        {
            case GameState.FIRST_NIGHT_MAFIA:
                startGameButton.gameObject.SetActive(false);
                timerPanel.gameObject.SetActive(true);
                timerPanel.SetState(visibleGameState);
                break;
            case GameState.FIRST_NIGHT_SHERIF:
                timerPanel.SetState(visibleGameState);
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
        roleContextMenu.transform.position = pos;
        roleContextMenu.SetActive(true);
    }

    private void OnDestroy()
    {
        Controller.singlton.onBackPressed -= HideContextMenu;
    }
}
