using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator
{
    private static Dictionary<Messages, string> firstDict;

    public static void Init()
    {
        firstDict = new Dictionary<Messages, string>();
        firstDict.Add(Messages.GAME_ALREADY_STARTED, "���� ��� ������");
        firstDict.Add(Messages.EMPTY_SLOT, "������ ����");
    }

    public static string Message(Messages message)
    {
        return firstDict[message];
    }

}
