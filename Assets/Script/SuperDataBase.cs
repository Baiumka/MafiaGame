

using System;
using System.Collections.Generic;

public class SuperDataBase : IDataBase 
{
    public bool IsLogin => throw new NotImplementedException();

    public void Login(string login, string password)
    {

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

        Random random = new Random();
        int randomIndex = random.Next(players.Count);
        return players[randomIndex];
    }

}
