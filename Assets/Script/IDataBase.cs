using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public delegate void UserInfoHandler(int number);

public interface IDataBase 
{    
    public bool IsLogin { get; }
    List<People> AvaiblePeople { get; }

    event UserInfoHandler OnUserLogin;
    event ErrorHandler OnDataBaseError;
    public void Login(string login, string password);

    
}
