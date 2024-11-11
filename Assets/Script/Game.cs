using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game
{
    private List<Player> players;
    
    public int Citizens { get => players.Count(player => player.Role == Role.CITIZEN || player.Role == Role.SHERIFF); }
    public int Mafia { get => players.Count(player => player.Role == Role.MAFIA || player.Role == Role.BOSS); }

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

    public bool CheckPlayer()
    {
        foreach(Player p in players)
        {
            if (p.Role == Role.NONE) return false;
            if (p.People == null) return false;
        }
        if (Mafia >= Citizens) return false;
        return true;
    }  
}
