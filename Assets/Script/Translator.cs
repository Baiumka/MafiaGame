using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator
{
    private static Dictionary<Messages, string> firstDict;

    public static void Init()
    {
        firstDict = new Dictionary<Messages, string>();
        firstDict.Add(Messages.GAME_ALREADY_STARTED, "Игра уже начата");
        firstDict.Add(Messages.EMPTY_SLOT, "Пустой слот");
    }

    public static string Message(Messages message)
    {
        return firstDict[message];
    }

}
