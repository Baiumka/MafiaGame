
using System.Collections.Generic;


public class Translator
{
    private static Dictionary<Messages, string> firstDict;
    private static Dictionary<EventType, string> actionsDict;

    public static void Init()
    {
        InitMainDict();
        InitActionDict();
    }

    private static void InitActionDict()
    {
        actionsDict = new Dictionary<EventType, string>();
        actionsDict.Add(EventType.KILL, "���� �����");
        actionsDict.Add(EventType.VOTE_TO_EXIT, "������ �������");
        actionsDict.Add(EventType.VOTE_TO_EXIT_BOTH, "������ �� �������");
        actionsDict.Add(EventType.SELF_VOTE, "������������ � ����");
        actionsDict.Add(EventType.SELF_KILL, "����� ����� ������");
        actionsDict.Add(EventType.PUT, "��������");
        actionsDict.Add(EventType.BOSS_CHECK, "�������� �� ���������");
        actionsDict.Add(EventType.SHERIF_CHECK, "���� �����");
        actionsDict.Add(EventType.BEST_TURN, "������� � ������ ���");
        actionsDict.Add(EventType.OUT, "����");
        actionsDict.Add(EventType.KICKED, "������ �������");
        actionsDict.Add(EventType.WARN, "������� ��������������");
        actionsDict.Add(EventType.WARN_EXIT, "������ 4 ��������������");
        actionsDict.Add(EventType.NEW_DAY, "�������� ����� ����");
    }

    private static void InitMainDict()
    {
        firstDict = new Dictionary<Messages, string>();
        firstDict.Add(Messages.MISS_SHOOTING_CONFIRM, "�� ������� ��� ����� ������������? ");
        firstDict.Add(Messages.SHOOTING_CONFIRM, "�� ������� ��� ��� ���� ���� �����? ");
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
        firstDict.Add(Messages.BOSS_CHEKING, "����� ��������. ����������� ��� � ���� ������!");
        firstDict.Add(Messages.BOSS_CONFIRM, "�� ������� ��� ��� ������ ������ ����� ������?");
        firstDict.Add(Messages.MISS_BOSS_CHECK_CONFIRM, "�� ������� ��� ��� ������ �� ��������?");
        firstDict.Add(Messages.SHERIF_CHEKING, "��� ��������. ����������� ����� � ���� �����!");
        firstDict.Add(Messages.SHERIF_CONFIRM, "�� ������� ��� ����� ������ ������ ����� ������?");
        firstDict.Add(Messages.MISS_SHERIF_CHECK_CONFIRM, "�� ������� ��� ����� ������ �� ��������?");
        firstDict.Add(Messages.BEST_TURN, "� ������� ������ ���� 20 ������ �� ������ ���.");
        firstDict.Add(Messages.MAFIA_SHOOTING, "����� ��������� ��� ������ � �������� ���� ���� �.....");
        firstDict.Add(Messages.WARN_CONFIRM, "�� ������� ��� ������ ������ ��������������?");
        firstDict.Add(Messages.KICK_CONFIRM, "�� ������� ��� ������ ������� ������?");
        firstDict.Add(Messages.MUTED_SPEAKER, ", �� �� ������ ��������, � ��� 3 ����. �� ����������� ����-������ �� �����������?");
        firstDict.Add(Messages.MAFIA_WIN, "������ �����!");
        firstDict.Add(Messages.CITIZEN_WIN, "������ ������!");
        firstDict.Add(Messages.NO_WIN, "����� =(");
        firstDict.Add(Messages.NO_END, "���� �� ��������");
        firstDict.Add(Messages.PUT_VOTE_CONFIRM, "�� ������� ��� ��������� ����� �������� ������?");
        firstDict.Add(Messages.ON_VOTE, "�� �����������:");
        firstDict.Add(Messages.CROSSFIRE_VOTE, "��� �� �� ��� �� ������� ����?");
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

    public static string Action(EventType action)
    {
        try
        {
            return actionsDict[action];
        }
        catch
        {
            return "Exception by tranlator on Message: " + action;
        }
    }

}
