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

    //private Game gameInfo;

    public void DrawMafiaWin(Game game)
    {
        headText.color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
        headText.text = Translator.Message(Messages.MAFIA_WIN);
        DrawResults(game);
    }

    public void DrawCitizenWin(Game game)
    {
        headText.color = ColorStore.store.MAFIA_BACKGROUND_COLOR;
        headText.text = Translator.Message(Messages.CITIZEN_WIN);
        DrawResults(game);
    }

    public void DrawNoWin(Game game)
    {
        headText.color = ColorStore.store.NONE_BACKGROUND_COLOR;
        headText.text = Translator.Message(Messages.NO_WIN);
        DrawResults(game);
    }

    private void DrawResults(Game game)
    {
        CleanLists();
        foreach (Player player in game.Players.Where(player => player.Role == Role.CITIZEN).ToList())
        {
            GameObject newObject = GameObject.Instantiate(playerShortPrefab, citizenList);
            PlayerShortObject playerShortObject = newObject.GetComponent<PlayerShortObject>();
            playerShortObject.Init(player);
        }

        foreach (Player player in game.Players.Where(player => player.Role == Role.MAFIA).ToList())
        {
            GameObject newObject = GameObject.Instantiate(playerShortPrefab, mafiaList);
            PlayerShortObject playerShortObject = newObject.GetComponent<PlayerShortObject>();
            playerShortObject.Init(player);
        }

        foreach (Player player in game.Players.Where(player => player.Role == Role.SHERIFF).ToList())
        {
            GameObject newObject = GameObject.Instantiate(playerShortPrefab, sherifList);
            PlayerShortObject playerShortObject = newObject.GetComponent<PlayerShortObject>();
            playerShortObject.Init(player);
        }

        foreach (Player player in game.Players.Where(player => player.Role == Role.BOSS).ToList())
        {
            GameObject newObject = GameObject.Instantiate(playerShortPrefab, bossList);
            PlayerShortObject playerShortObject = newObject.GetComponent<PlayerShortObject>();
            playerShortObject.Init(player);
        }
    }

    private void CleanLists()
    {
        foreach (Transform child in citizenList) Destroy(child.gameObject);
        foreach (Transform child in mafiaList) Destroy(child.gameObject);
        foreach (Transform child in sherifList) Destroy(child.gameObject);
        foreach (Transform child in bossList) Destroy(child.gameObject);
    }

    internal void DrawResults(List<ResultPlayer> players, List<ResultHistoryEvent> history, GameInfo game)
    {
        throw new NotImplementedException();
    }
}

