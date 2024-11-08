
using System;


public class Player 
{
    private int number;
    private People people;

    public VoidHandler onDataUpdated;

    public Player(int number)
    {
        this.number = number;
    }

    public int Number { get => number;}
    public People People { get => people;}

    public void SetPeople(People people)
    {
        this.people = people;
        onDataUpdated?.Invoke();
    }
}
