

using System;
using System.Collections.Generic;

public class Database 
{
    public static List<People> AvaiblePeople 
    { 
        get 
        {
            List<People> result = new List<People>();
            result.Add(new People("Aldric"));
            result.Add(new People("Brynna"));
            result.Add(new People("Cedric"));
            result.Add(new People("Darian"));
            result.Add(new People("Elara"));
            result.Add(new People("Faelan"));
            result.Add(new People("Gareth"));
            result.Add(new People("Halwyn"));
            result.Add(new People("Ilyana"));
            result.Add(new People("Jareth"));
            result.Add(new People("Kaelin"));
            result.Add(new People("Lira"));
            result.Add(new People("Maelis"));
            result.Add(new People("Nerys"));
            result.Add(new People("Orin"));
            result.Add(new People("Perrin"));
            result.Add(new People("Quinn"));
            result.Add(new People("Riva"));
            result.Add(new People("Seren"));
            result.Add(new People("Theron"));
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
