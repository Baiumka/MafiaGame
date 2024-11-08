using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWindow : MonoBehaviour
{
    [SerializeField] Transform playerPanel;
    [SerializeField] GameObject playerPrefab;

    private List<PlayerObject> playersList;
    public void InitGame(Game game)
    {
        playersList = new List<PlayerObject>(game.Players.Count);
        foreach(Player player in game.Players)
        {
            GameObject newPlayerObject = GameObject.Instantiate(playerPrefab, playerPanel);
            PlayerObject po = newPlayerObject.GetComponent<PlayerObject>();
            po.Init(player);
            playersList.Add(po);    


        }
    }
}
