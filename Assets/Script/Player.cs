
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public delegate void VoteHandler();
public class Player 
{
    private int number;
    private People people;
    private Role role = Role.NONE;
    private bool isDead = false;
    private bool isPutted = false;
    private Player voted;
    private bool isBestTurn;
    private int warn;

    [JsonIgnore] public VoidHandler onDataUpdated;
    [JsonIgnore] public VoteHandler onPlayerPut;
    [JsonIgnore] public VoteHandler onPlayerVote;
    [JsonIgnore] public VoteHandler onPlayerDie;
    [JsonIgnore] public VoteHandler onPreparedForDie;
    [JsonIgnore] public VoteHandler onAddedToBestTurn;
    [JsonIgnore] public VoidHandler onTakeWarn;

    public Player(int number)
    {
        this.number = number;
        //this.people = PHPDatabase.GetRandomPlayer();
    }

    public int Number { get => number;}
    public People People { get => people;}
    [JsonConverter(typeof(StringEnumConverter))] public Role Role { get => role; }
    public bool IsDead { get => isDead; }
    [JsonIgnore] public bool IsPutted { get => isPutted; }
    [JsonIgnore] public Player Voted { get => voted; }
    [JsonIgnore] public bool IsBestTurn { get => isBestTurn; }
    public int Warn { get => warn; }

    public void Put()
    {
        isPutted = true;
        onPlayerPut?.Invoke();
    }

    public void Vote(Player player)
    {
        voted = player;
        onPlayerVote?.Invoke();
    }


    public void Die()
    {
        isDead = true;
        onPlayerDie?.Invoke();
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

    public void UnPut()
    {
        isPutted = false;
        onDataUpdated?.Invoke();
    }

    public void UnVote()
    {
        voted = null;
        onDataUpdated?.Invoke();
    }

    public void PrepareForDie()
    {
        onPreparedForDie?.Invoke();
    }

    public void AddBestTurn()
    {
        isBestTurn = true;
        onAddedToBestTurn?.Invoke();
    }

    public void RemoveBestTurn()
    {
        isBestTurn = false;
        onDataUpdated?.Invoke();
    }

    internal void GiveWarn()
    {
        warn++;
        onTakeWarn?.Invoke();
    }
}
