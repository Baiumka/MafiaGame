using System;
using UnityEngine;

public class GameInfo 
{
    public DateTime dateTime;
    public int number;
    public string winner;

    public GameInfo(DateTime dateTime, int number, string winner)
    {
        this.dateTime = dateTime;
        this.number = number;
        this.winner = winner;
    }
}
