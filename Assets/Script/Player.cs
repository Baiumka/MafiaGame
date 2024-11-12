
using System;

public delegate void VoteHandler();
public class Player 
{
    private int number;
    private People people;
    private Role role = Role.NONE;
    private bool isDead = false;
    private bool isVoted = false;


    public VoidHandler onDataUpdated;
    public VoteHandler onPlayerVoted;

    public Player(int number)
    {
        this.number = number;
        this.people = Database.GetRandomPlayer();
    }

    public int Number { get => number;}
    public People People { get => people;}
    public Role Role { get => role; }
    public bool IsDead { get => isDead; }
    public bool IsVoted { get => isVoted; }

    public void Vote()
    {
        isVoted = true;
        onPlayerVoted?.Invoke();
    }

    public void Die()
    {
        isDead = true;
        onDataUpdated?.Invoke();
    }

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

    public void UnVote()
    {
        isVoted = false;
        onDataUpdated?.Invoke();
    }
}
