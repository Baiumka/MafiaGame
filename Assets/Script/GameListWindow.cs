using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameListWindow : MonoBehaviour
{
    [SerializeField] private GameObject gameItemPrefab;
    [SerializeField] private Transform listTranform;

    public void Init()
    {
        Controller.singlton.onGameListRefreshed += DrawList;
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
