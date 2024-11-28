

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

        Random random = new Random();
        int randomIndex = random.Next(players.Count);
        return players[randomIndex];
    }

}
