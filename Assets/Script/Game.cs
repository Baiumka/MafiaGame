using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    private List<Player> players;
    
    public Game(int playerCount)
    {
        players = new List<Player>(playerCount);
        for(int i = 1; i <= playerCount; i++)
        {
            players.Add(new Player(i));
        }        
    }

    public List<Player> Players { get => players; }

    public void SetPlayer(Player player, People people)
    {
        foreach(Player p in players)
        {
            if(p.Equals(player))
            {
                p.SetPeople(people);
                return;
            }
        }
    }
}
