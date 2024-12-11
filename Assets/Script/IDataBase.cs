using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public delegate void UserInfoHandler(int number, string name);
public delegate void PeopleInfoHandler(People people);

public interface IDataBase 
{    
    public bool IsLogin { get; }
    List<People> AvaiblePeople { get; }

    event UserInfoHandler OnUserLogin;
    event ErrorHandler OnDataBaseError;
    event PeopleInfoHandler OnPeopleAdded;

    public void AddPeople(string name);
    public void Login(string login, string password);
    public void Register(string login, string password, string name);
    public void WriteResult(Game game);
}
