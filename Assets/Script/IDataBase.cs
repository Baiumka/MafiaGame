using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public delegate void UserInfoHandler(int number, string name);
public delegate void PeopleInfoHandler(People people);
public delegate void GameListHandler(List<GameInfo> gameList);
public delegate void GameDetailsHandler(List<ResultPlayer> players, List<ResultHistoryEvent> history, GameInfo game);

public interface IDataBase 
{    
    public bool IsLogin { get; }
    List<People> AvaiblePeople { get; }

    event UserInfoHandler OnUserLogin;
    event ErrorHandler OnDataBaseError;
    event PeopleInfoHandler OnPeopleAdded;
    event GameListHandler OnGameListRefreshed;
    event GameDetailsHandler OnGameDetailsUpdated;

    public void AddPeople(string name);
    public void Login(string login, string password);
    public void Register(string login, string password, string name);
    public void StartNewGame(Game game);
    public void WriteHistory(History.HistoryEvent historyEvent);
    public void EndGame(Game game, int winner);
    public void RefreshGameList();
    public void GetGameDetails(GameInfo game);

}
