using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game
{
    private List<Player> players;
    public int Citizens { get => players.Count(player => (player.Role == Role.CITIZEN || player.Role == Role.SHERIFF) && !player.IsDead); }
    public int Mafia { get => players.Count(player => (player.Role == Role.MAFIA || player.Role == Role.BOSS) && !player.IsDead); }
    public List<Player> VotedPlayers { get => players.Where(player => player.IsVoted && !player.IsDead).ToList(); }


    public Game(int playerCount)
    {
        players = new List<Player>(playerCount);
        for(int i = 1; i <= playerCount; i++)
        {
            players.Add(new Player(i));
        }        
    }

    public List<Player> Players { get => players; }

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

    public Player GetPlayerByNumber(int n)
    {
        Player answer = null;
        int i = 0;
        while(answer == null)
        {
            Player p = players[i];
            if (p.Number == n)
            {
                if (!p.IsDead)
                {
                    answer = p;
                    break;
                }
                else
                {
                    n++;
                }
            }
            i++;
            if (i >= players.Count) i = 0;
            if (n > players.Count) n = 1;
        }
        return answer;
    }
}
