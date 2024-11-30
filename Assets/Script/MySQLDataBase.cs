

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;


public class MySQLDataBase : IDataBase 
{
    public bool IsLogin => throw new NotImplementedException();

    public void Test()
    {
        // Строка подключения
        string connectionString = "Server=junction.proxy.rlwy.net;Port=43851;Database=railway;User ID=root;Password=XEhtSYcfTgVtnbpOOyUbJXSjVSukSEdo;";



        // Создаем подключение
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                // Открываем подключение
                connection.Open();
                UnityEngine.Debug.Log("Подключение установлено!");

                // Пример выполнения запроса
                string query = "SELECT * FROM users";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UnityEngine.Debug.Log($"ID: {reader["id"]}, Name: {reader["name"]}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log($"Ошибка подключения: {ex.Message}, {connection.Database}");
                if (ex.InnerException != null)
                {
                    UnityEngine.Debug.Log($"Вложенная ошибка: {ex.InnerException.Message}");
                }
            }
        }

    }




    public static List<People> AvaiblePeople 
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

    public static People GetRandomPlayer()
    {
        List<People> players = AvaiblePeople;
        if (players == null || players.Count == 0)
        {
            return null; // или выбросьте исключение, если список пуст
        }

        System.Random random = new System.Random();
        int randomIndex = random.Next(players.Count);
        return players[randomIndex];
    }

}
