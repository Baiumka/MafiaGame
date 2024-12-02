

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using UnityEditor.Search;

public class MySQLDataBase : IDataBase 
{
    private const string CONNECTION_STRING = "Server=junction.proxy.rlwy.net;Port=43851;Database=railway;User ID=root;Password=XEhtSYcfTgVtnbpOOyUbJXSjVSukSEdo;";
    private MySqlConnection connection;
    private bool isLogin;
    private int userID = 0;
    private List<People> peopleList;
    public event UserInfoHandler OnUserLogin;
    public event PeopleInfoHandler OnPeopleAdded;
    public event ErrorHandler OnDataBaseError;

    private MySqlConnection Conn 
    {
        get 
        {
            if (connection == null)
                return Connect();
            else if (!connection.Ping())
                return Connect();
            else
                return connection;
        }
    }
    public List<People> AvaiblePeople => peopleList;
    public bool IsLogin => true;//Заменить на isLogin

    public MySQLDataBase()
    {
        peopleList = new List<People>();
    }

    private void LoadPeopleList()
    {
        try
        {
            string query = "SELECT id, name FROM people WHERE id_user = " + userID;
            OnDataBaseError?.Invoke(query);             
            using (MySqlCommand cmd = new MySqlCommand(query, Conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Читаем данные из таблицы
                        int id = reader.GetInt32("id");
                        string name = reader.GetString("name");
                        OnDataBaseError?.Invoke(name);
                        peopleList.Add(new People(id, name));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            OnDataBaseError?.Invoke($"Ошибка выполнения запроса: {ex.Message}");
            if (ex.InnerException != null)
            {
                OnDataBaseError?.Invoke($"Вложенная ошибка: {ex.InnerException.Message}");
            }
        }
    }

    private MySqlConnection Connect()
    {
        try
        {
            connection = new MySqlConnection(CONNECTION_STRING);
            connection.Open();
            return connection;
        }
        catch (Exception ex)
        {
            OnDataBaseError?.Invoke($"Connect Error: {ex.Message}, {connection.Database}");
            if (ex.InnerException != null)
            {
                OnDataBaseError?.Invoke($"Inner Error: {ex.InnerException.Message}");
            }
            return null;
        }
    }

    private void SuccssesLogin(int id, int userNumber)
    {        
        OnUserLogin?.Invoke(userNumber);
        userID = id;
        LoadPeopleList();
        isLogin = true;
    }

    public void Login(string login, string password)
    {
        try
        {
            // Проверяем, существует ли пользователь с таким логином
            string queryCheckUser = "SELECT number, password,id FROM users WHERE login = @login";
            using (MySqlCommand cmdCheckUser = new MySqlCommand(queryCheckUser, Conn))
            {
                cmdCheckUser.Parameters.AddWithValue("@login", login);
                int userNumber = 0;
                int id = 0;
                using (MySqlDataReader reader = cmdCheckUser.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Пользователь найден, проверяем пароль
                        string storedPassword = reader.GetString("password");                        
                        if (storedPassword == password)
                        {
                            userNumber = reader.GetInt32("number");
                            id = reader.GetInt32("id");                            
                        }
                        else
                        {
                            OnDataBaseError?.Invoke(Translator.Message(Messages.WRONG_PASSWORD)); ;
                        }
                    }
                }
                if(userNumber != 0 && id != 0) 
                {
                    SuccssesLogin(id, userNumber);
                    return;
                }
            }

            // Пользователь не найден, создаем нового
            string queryGetMaxNumber = "SELECT MAX(number) FROM users";
            int newNumber = 1;

            using (MySqlCommand cmdGetMaxNumber = new MySqlCommand(queryGetMaxNumber, Conn))
            {
                object result = cmdGetMaxNumber.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    newNumber = Convert.ToInt32(result) + 1;
                }
            }

            // Добавляем нового пользователя
            string queryInsertUser = "INSERT INTO users (number, login, password) VALUES (@number, @login, @password); SELECT LAST_INSERT_ID();";
            using (MySqlCommand cmdInsertUser = new MySqlCommand(queryInsertUser, Conn))
            {
                cmdInsertUser.Parameters.AddWithValue("@number", newNumber);
                cmdInsertUser.Parameters.AddWithValue("@login", login);
                cmdInsertUser.Parameters.AddWithValue("@password", password);

                int newId = Convert.ToInt32(cmdInsertUser.ExecuteScalar());
                SuccssesLogin(newId, newNumber);
            }
        }
        catch (Exception ex)
        {
            // Обработка ошибок
            OnDataBaseError?.Invoke($"Login error: {ex.Message}");
            if (ex.InnerException != null)
            {
                OnDataBaseError?.Invoke($"Inner Error: {ex.InnerException.Message}");
            }
        }
        finally
        {
            
        }
    }


    

    public static People GetRandomPlayer()
    {
        List<People> players = Controller.AvaiblePeople;
        if (players == null || players.Count == 0)
        {
            return null; // или выбросьте исключение, если список пуст
        }

        System.Random random = new System.Random();
        int randomIndex = random.Next(players.Count);
        return players[randomIndex];
    }

    public void AddPeople(string name)
    {
        if (isLogin)
        {
            try
            {
                string queryInsertPeople = "INSERT INTO people (name, id_user) VALUES (@name, @id_user); SELECT LAST_INSERT_ID();";
                using (MySqlCommand cmdInsertPeople = new MySqlCommand(queryInsertPeople, Conn))
                {
                    cmdInsertPeople.Parameters.AddWithValue("@name", name);
                    cmdInsertPeople.Parameters.AddWithValue("@id_user", userID);
                    int newId = Convert.ToInt32(cmdInsertPeople.ExecuteScalar());
                    People newPeople = new People(newId, name);
                    peopleList.Add(newPeople);
                    OnPeopleAdded?.Invoke(newPeople);
                }
            }
            catch (Exception ex)
            {
                OnDataBaseError?.Invoke($"Connect Error: {ex.Message}, {connection.Database}");
                if (ex.InnerException != null)
                {
                    OnDataBaseError?.Invoke($"Inner Error: {ex.InnerException.Message}");
                }                
            }
        }
    }
}
