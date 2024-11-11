using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator
{
    private static Dictionary<Messages, string> firstDict;

    public static void Init()
    {
        firstDict = new Dictionary<Messages, string>();
        firstDict.Add(Messages.FIRST_NIGHT_SHERIF, "Просыпается шериф, у него есть время что бы осмотреть город и поприветствовать ведущего!");
        firstDict.Add(Messages.FIRST_NIGHT_SHERIF_END, "Шериф засыпает.");
        firstDict.Add(Messages.FIRST_NIGHT_MAFIA, "Просыпается мафия, у неё есть минута что бы договориться!");
        firstDict.Add(Messages.FIRST_NIGHT_MAFIA_END, "Мафия засыпает.");
        firstDict.Add(Messages.GAME_ALREADY_STARTED, "Игра уже начата");
        firstDict.Add(Messages.EMPTY_SLOT, "Пустой слот");
        firstDict.Add(Messages.ERROR_CHECK_PLAYER, "Проверьте правильность заполнения стола.");
    }

    public static string Message(Messages message)
    {
        return firstDict[message];
    }

}
