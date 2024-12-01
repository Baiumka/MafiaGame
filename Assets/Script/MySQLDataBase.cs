

using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;


public class MySQLDataBase : IDataBase 
{
    private const string CONNECTION_STRING = "Server=junction.proxy.rlwy.net;Port=43851;Database=railway;User ID=root;Password=XEhtSYcfTgVtnbpOOyUbJXSjVSukSEdo;";
    private MySqlConnection connection;

    public event UserInfoHandler OnUserLogin;
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
    public List<People> AvaiblePeople
    {
        get
        {
            List<People> result = new List<People>();
            result.Add(new People("Ма"));
            result.Add(new People("Карабас"));
            result.Add(new People("Vera"));
            result.Add(new People("Vilegecko"));
            result.Add(new People("Квітка"));
            result.Add(new People("Туманнушка"));
            result.Add(new People("Неплюшевый Мишка"));
            result.Add(new People("Ангел"));
            result.Add(new People("Батюшка"));
            result.Add(new People("Бадумка"));
            result.Add(new People("Князь"));
            return result;
        }
    }

    public bool IsLogin => throw new NotImplementedException();

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
            OnDataBaseError?.Invoke($"Ошибка подключения: {ex.Message}, {connection.Database}");
            if (ex.InnerException != null)
            {
                OnDataBaseError?.Invoke($"Вложенная ошибка: {ex.InnerException.Message}");
            }
            return null;
        }
    }

    public void Login(string login, string password)
    {
        try
        {
            // Проверяем, существует ли пользователь с таким логином
            string queryCheckUser = "SELECT number, password FROM users WHERE login = @login";
            using (MySqlCommand cmdCheckUser = new MySqlCommand(queryCheckUser, Conn))
            {
                cmdCheckUser.Parameters.AddWithValue("@login", login);

                using (MySqlDataReader reader = cmdCheckUser.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Пользователь найден, проверяем пароль
                        string storedPassword = reader.GetString("password");
                        int userNumber = reader.GetInt32("number");

                        if (storedPassword == password)
                        {
                            OnUserLogin?.Invoke(userNumber);
                        }
                        else
                        {
                           OnDataBaseError?.Invoke("Неправильный пароль.");
                        }
                        return;
                    }
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
            string queryInsertUser = "INSERT INTO users (number, login, password) VALUES (@number, @login, @password)";
            using (MySqlCommand cmdInsertUser = new MySqlCommand(queryInsertUser, Conn))
            {
                cmdInsertUser.Parameters.AddWithValue("@number", newNumber);
                cmdInsertUser.Parameters.AddWithValue("@login", login);
                cmdInsertUser.Parameters.AddWithValue("@password", password);

                cmdInsertUser.ExecuteNonQuery();
                OnUserLogin?.Invoke(newNumber);
            }
        }
        catch (Exception ex)
        {
            // Обработка ошибок
            OnDataBaseError?.Invoke($"Ошибка при логине: {ex.Message}");
            if (ex.InnerException != null)
            {
                OnDataBaseError?.Invoke($"Вложенная ошибка: {ex.InnerException.Message}");
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

    
}
