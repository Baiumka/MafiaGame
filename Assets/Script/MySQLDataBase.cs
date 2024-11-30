

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
        // ������ �����������
        string connectionString = "Server=junction.proxy.rlwy.net;Port=43851;Database=railway;User ID=root;Password=XEhtSYcfTgVtnbpOOyUbJXSjVSukSEdo;";



        // ������� �����������
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                // ��������� �����������
                connection.Open();
                UnityEngine.Debug.Log("����������� �����������!");

                // ������ ���������� �������
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
                UnityEngine.Debug.Log($"������ �����������: {ex.Message}, {connection.Database}");
                if (ex.InnerException != null)
                {
                    UnityEngine.Debug.Log($"��������� ������: {ex.InnerException.Message}");
                }
            }
        }

    }




    public static List<People> AvaiblePeople 
    { 
        get 
        {
            List<People> result = new List<People>();
            result.Add(new People("��"));
            result.Add(new People("�������"));
            result.Add(new People("Vera"));
            result.Add(new People("Vilegecko"));
            result.Add(new People("�����"));
            result.Add(new People("����������"));
            result.Add(new People("���������� �����"));
            result.Add(new People("�����"));
            result.Add(new People("�������"));
            result.Add(new People("�������"));
            result.Add(new People("�����"));
            return result;   
        } 
    }

    public static People GetRandomPlayer()
    {
        List<People> players = AvaiblePeople;
        if (players == null || players.Count == 0)
        {
            return null; // ��� ��������� ����������, ���� ������ ����
        }

        System.Random random = new System.Random();
        int randomIndex = random.Next(players.Count);
        return players[randomIndex];
    }

}
