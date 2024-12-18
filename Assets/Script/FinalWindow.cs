using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class FinalWindow : MonoBehaviour
{
    [SerializeField] private GameObject playerShortPrefab;
    [SerializeField] private TMP_Text headText;
    [SerializeField] private Transform citizenList;
    [SerializeField] private Transform mafiaList;
    [SerializeField] private Transform sherifList;
    [SerializeField] private Transform bossList;
    [SerializeField] private Transform logList;
    [SerializeField] private GameObject logItemPrefab;
    [SerializeField] private Color color1;
    [SerializeField] private Color color2;
    [SerializeField] private ShortNumberPrefab firstKillNumber;
    [SerializeField] private TMP_Text firstKillNameText;
    [SerializeField] private Transform bestTurnTranform;
    [SerializeField] private GameObject shortNumberPrefab;
    [SerializeField] private Button exitButton;

    private List<ResultHistoryEvent> bestTurn;

    public void Init()
    {
        exitButton.onClick.AddListener(OnLickExitButton);
    }

    private void OnLickExitButton()
    {
        Controller.singlton.ReturnBack();
    }

    private void DrawMafiaWin()
    {
        headText.color = ColorStore.store.MAFIA_TEXT_COLOR;
        headText.transform.parent.GetComponent<Image>().color = ColorStore.store.MAFIA_BACKGROUND_COLOR;
        headText.text = Translator.Message(Messages.MAFIA_WIN);
        
    }

    private void DrawCitizenWin()
    {
        headText.color = ColorStore.store.CITIZEN_TEXT_COLOR;
        headText.transform.parent.GetComponent<Image>().color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
        headText.text = Translator.Message(Messages.CITIZEN_WIN);
    }

    private void DrawNoWin()
    {
        headText.color = ColorStore.store.NONE_TEXT_COLOR;
        headText.transform.parent.GetComponent<Image>().color = ColorStore.store.NONE_BACKGROUND_COLOR;
        headText.text = Translator.Message(Messages.NO_WIN);
    }


    private void CleanLists()
    {
        foreach (Transform child in citizenList) Destroy(child.gameObject);
        foreach (Transform child in mafiaList) Destroy(child.gameObject);
        foreach (Transform child in sherifList) Destroy(child.gameObject);
        foreach (Transform child in bossList) Destroy(child.gameObject);
        foreach (Transform child in logList) Destroy(child.gameObject);
        foreach (Transform child in bestTurnTranform) Destroy(child.gameObject);
    }

    

    internal void DrawResults(List<ResultPlayer> players, List<ResultHistoryEvent> history, GameInfo game)
    {
        CleanLists();

        switch(game.winner)
        {
            case 1:
                DrawCitizenWin();
                break;
            case 2:
                DrawMafiaWin();
                break;
            default:
                DrawNoWin();
                break;
        }



        foreach (ResultPlayer player in players.Where(player => player.role == Role.CITIZEN).ToList())
        {
            GameObject newObject = GameObject.Instantiate(playerShortPrefab, citizenList);
            PlayerShortObject playerShortObject = newObject.GetComponent<PlayerShortObject>();
            playerShortObject.Init(player);
        }

        foreach (ResultPlayer player in players.Where(player => player.role == Role.MAFIA).ToList())
        {
            GameObject newObject = GameObject.Instantiate(playerShortPrefab, mafiaList);
            PlayerShortObject playerShortObject = newObject.GetComponent<PlayerShortObject>();
            playerShortObject.Init(player);
        }

        foreach (ResultPlayer player in players.Where(player => player.role == Role.SHERIFF).ToList())
        {
            GameObject newObject = GameObject.Instantiate(playerShortPrefab, sherifList);
            PlayerShortObject playerShortObject = newObject.GetComponent<PlayerShortObject>();
            playerShortObject.Init(player);
        }

        foreach (ResultPlayer player in players.Where(player => player.role == Role.BOSS).ToList())
        {
            GameObject newObject = GameObject.Instantiate(playerShortPrefab, bossList);
            PlayerShortObject playerShortObject = newObject.GetComponent<PlayerShortObject>();
            playerShortObject.Init(player);
        }

        bestTurn = new List<ResultHistoryEvent>();
        int i = 1;
        foreach(ResultHistoryEvent e in history)
        {
            if (e.event_type == EventType.WARN) continue;
            if (e.event_type == EventType.BEST_TURN)
            {
                bestTurn.Add(e);
                continue;
            }
            GameObject eventObject = GameObject.Instantiate(logItemPrefab, logList);
            LogItem logItem = eventObject.GetComponent<LogItem>();
            logItem.Init(e);
            logItem.SetColor((i % 2 == 1 ? color1 : color2 ));
            i++;
            
        }
      
        foreach (ResultHistoryEvent e in bestTurn)
        {
            GameObject shortObject = GameObject.Instantiate(shortNumberPrefab, bestTurnTranform);
            ShortNumberPrefab prefab = shortObject.GetComponent<ShortNumberPrefab>();
            prefab.SetNumber(e.target_number, e.target_role);
            firstKillNumber.SetNumber(e.player_number, e.player_role);
            firstKillNameText.text = e.player_name;
        }



    }
}

