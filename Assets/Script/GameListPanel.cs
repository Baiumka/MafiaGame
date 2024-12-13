using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameListPanel : MonoBehaviour
{
    [SerializeField] private GameObject gameItemPrefab;
    [SerializeField] private Transform listTranform;

    public void DrawList(List<GameInfo> gameList)
    {
        foreach (GameInfo gameInfo in gameList)
        {

        }
    }
}
