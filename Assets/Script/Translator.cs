using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator
{
    private static Dictionary<Messages, string> firstDict;

    public static void Init()
    {
        firstDict = new Dictionary<Messages, string>();
        firstDict.Add(Messages.YOUR_LAST_WORD, ", � ��� ������ �� ��������� �����.");
        firstDict.Add(Messages.VOTE_FOR_UP, "��� �� �� ��� �� ������� �����?");
        firstDict.Add(Messages.DOP_SPEAK_OFFICIAL2, "� ��� ���� �� 30 ������.");
        firstDict.Add(Messages.DOP_SPEAK_OFFICIAL1, "� ��� ����������� ����� ��������: ");
        firstDict.Add(Messages.TOO_LATE_UNVOTE, "���� ����� ��������� � ������� ������. �������� ��� ����� ������.");
        firstDict.Add(Messages.VOTES_COUNT, "�������: ");
        firstDict.Add(Messages.VOTE, "��� ������ ������ �");
        firstDict.Add(Messages.CROSSFIRE, "�����������");
        firstDict.Add(Messages.VOTE_OFFICIAL1, "�� ���������� ���������� ������: ");        
        firstDict.Add(Messages.VOTE_OFFICIAL2, "�������� ������ � ����� �������: ");        
        firstDict.Add(Messages.ALREADY_PUTTED, "����� ��� ��������� ����-��");
        firstDict.Add(Messages.ALREADY_VOTED, "����� ��� ��������� �� �����������");
        firstDict.Add(Messages.ALREADY_VOTED_VOTE, "���� ����� ��� ���������.");
        firstDict.Add(Messages.ALREADY_DEAD, "���� ����� ��� ����.");
        firstDict.Add(Messages.COURT, "���");
        firstDict.Add(Messages.DAY, "���� ");
        firstDict.Add(Messages.NIGHT, "����");
        firstDict.Add(Messages.START_NIGHT, "� ������ ��������� ����. ��� ��������.");
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
        try
        {
            return firstDict[message];
        }
        catch 
        {
            return "Exception by tranlator on Message: " + message;
        }
    }

}
