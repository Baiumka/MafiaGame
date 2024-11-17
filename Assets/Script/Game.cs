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
    public List<Player> PuttedPlayers { get => players.Where(player => player.IsPutted && !player.IsDead).ToList(); }
    public List<Player> AlivePlayers { get => players.Where(player => !player.IsDead).ToList(); }


    public Game(int playerCount)
    {
        players = new List<Player>(playerCount);
        for(int i = 1; i <= playerCount; i++)
        {
            Player newPlayer = new Player(i);
            players.Add(newPlayer);
            if (i == 1) newPlayer.SetRole(Role.CITIZEN);
            if (i == 2) newPlayer.SetRole(Role.CITIZEN);
            if (i == 3) newPlayer.SetRole(Role.CITIZEN);
            if (i == 4) newPlayer.SetRole(Role.CITIZEN);
            if (i == 5) newPlayer.SetRole(Role.CITIZEN);
            if (i == 6) newPlayer.SetRole(Role.CITIZEN);
            if (i == 7) newPlayer.SetRole(Role.SHERIFF);
            if (i == 8) newPlayer.SetRole(Role.MAFIA);
            if (i == 9) newPlayer.SetRole(Role.MAFIA);
            if (i == 10) newPlayer.SetRole(Role.BOSS);
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

    public Player GetNextPlayer(Player player)
    {
        if (player == null) return Players[0];
        Player nextPlayer = player;

        while (nextPlayer.IsDead || nextPlayer == player)
        {            
            nextPlayer = players.Where(p => (p.Number == nextPlayer.Number + 1 || (p.Number == 1 && nextPlayer.Number == Players.Count))).First();
        }
        return nextPlayer;       
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
