using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameListWindow : MonoBehaviour
{
    [SerializeField] private GameObject gameItemPrefab;
    [SerializeField] private Transform listTranform;
    [SerializeField] private Button exitButton;

    public void Init()
    {
        exitButton.onClick.AddListener(OnLickExitButton);
        Controller.singlton.onGameListRefreshed += DrawList;
    }

    private void OnLickExitButton()
    {
        Controller.singlton.ReturnBack();
        exitButton.onClick.AddListener(OnLickExitButton);
    }

    public void DrawList(List<GameInfo> gameList)
    {
        Clean();
        foreach (GameInfo gameInfo in gameList)
        {
            GameObject gameObject = GameObject.Instantiate(gameItemPrefab, listTranform);
            GameListItem item = gameObject.GetComponent<GameListItem>();
            item.Init(gameInfo);
        }
    }

    private void Clean()
    {
        foreach (Transform child in listTranform)
        {
            Destroy(child.gameObject);
        }
    }
}
