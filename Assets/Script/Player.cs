
using System;


public class Player 
{
    private int number;
    private People people;
    private Role role = Role.NONE;

    public VoidHandler onDataUpdated;

    public Player(int number)
    {
        this.number = number;
    }

    public int Number { get => number;}
    public People People { get => people;}
    public Role Role { get => role; }

    public void SetPeople(People people)
    {
        this.people = people;
        onDataUpdated?.Invoke();
    }

    public void SetRole(Role role)
    {
        this.role = role;
        onDataUpdated?.Invoke();
    }
}
