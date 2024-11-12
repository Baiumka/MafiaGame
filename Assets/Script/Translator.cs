using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator
{
    private static Dictionary<Messages, string> firstDict;

    public static void Init()
    {
        firstDict = new Dictionary<Messages, string>();
        firstDict.Add(Messages.VOTE_OFFICIAL1, "�� ����������� ���������� ������: ");
        firstDict.Add(Messages.VOTE_OFFICIAL2, "�������� ������ � ����� �������: ");
        firstDict.Add(Messages.ALREADY_PUTTED, "����� ��� ��������� ����-��");
        firstDict.Add(Messages.ALREADY_VOTED, "����� ��� ��������� �� �����������");
        firstDict.Add(Messages.COURT, "���");
        firstDict.Add(Messages.DAY, "���� ");
        firstDict.Add(Messages.MORNING, "������ ����!");
        firstDict.Add(Messages.SPEAKER, "������� ����� �");
        firstDict.Add(Messages.FIRST_NIGHT, "������ ����");
        firstDict.Add(Messages.FIRST_NIGHT_SHERIF, "����������� �����, � ���� ���� ����� ��� �� ��������� ����� � ���������������� ��������!");
        firstDict.Add(Messages.FIRST_NIGHT_SHERIF_END, "����� ��������.");
        firstDict.Add(Messages.FIRST_NIGHT_MAFIA, "����������� �����, � �� ���� ������ ��� �� ������������!");
        firstDict.Add(Messages.FIRST_NIGHT_MAFIA_END, "����� ��������.");
        firstDict.Add(Messages.GAME_ALREADY_STARTED, "���� ��� ������");
        firstDict.Add(Messages.EMPTY_SLOT, "������ ����");
        firstDict.Add(Messages.ERROR_CHECK_PLAYER, "��������� ������������ ���������� �����.");
    }

    public static string Message(Messages message)
    {
        return firstDict[message];
    }

}
