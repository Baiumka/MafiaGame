using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator
{
    private static Dictionary<Messages, string> firstDict;

    public static void Init()
    {
        firstDict = new Dictionary<Messages, string>();
        firstDict.Add(Messages.MISS_SHOOTING_CONFIRM, "Вы уверены что мафия промахнулась? ");
        firstDict.Add(Messages.SHOOTING_CONFIRM, "Вы уверены что был убит этот игрок? ");
        firstDict.Add(Messages.YOUR_LAST_WORD, ", у вас минута на последнее слово.");
        firstDict.Add(Messages.VOTE_FOR_UP, "Кто за то что бы поднять обоих?");
        firstDict.Add(Messages.DOP_SPEAK_OFFICIAL2, "У них есть по 30 секунд.");
        firstDict.Add(Messages.DOP_SPEAK_OFFICIAL1, "У нас перестрелка между игроками: ");
        firstDict.Add(Messages.TOO_LATE_UNVOTE, "Этот игрок голосовал в другого игрока. Отменить его голос нельзя.");
        firstDict.Add(Messages.VOTES_COUNT, "Голосов: ");
        firstDict.Add(Messages.VOTE, "Кто против игрока №");
        firstDict.Add(Messages.CROSSFIRE, "Перестрелка");
        firstDict.Add(Messages.VOTE_OFFICIAL1, "На голосвание выставлены игроки: ");        
        firstDict.Add(Messages.VOTE_OFFICIAL2, "Голосуем именно в таком порядке: ");        
        firstDict.Add(Messages.ALREADY_PUTTED, "Игрок уже выставлял кого-то");
        firstDict.Add(Messages.ALREADY_VOTED, "Игрок уже выставлен на голосование");
        firstDict.Add(Messages.ALREADY_VOTED_VOTE, "Этот игрок уже голосовал.");
        firstDict.Add(Messages.ALREADY_DEAD, "Этот игрок уже мёртв.");
        firstDict.Add(Messages.COURT, "СУД");
        firstDict.Add(Messages.DAY, "День ");
        firstDict.Add(Messages.NIGHT, "Ночь");
        firstDict.Add(Messages.START_NIGHT, "В городе наступает ночь. Все засыпают.");
        firstDict.Add(Messages.MORNING, "Доброе утро!");
        firstDict.Add(Messages.SPEAKER, "Говорит игрок №");
        firstDict.Add(Messages.FIRST_NIGHT, "Первая Ночь");
        firstDict.Add(Messages.FIRST_NIGHT_SHERIF, "Просыпается шериф, у него есть время что бы осмотреть город и поприветствовать ведущего!");
        firstDict.Add(Messages.FIRST_NIGHT_SHERIF_END, "Шериф засыпает.");
        firstDict.Add(Messages.FIRST_NIGHT_MAFIA, "Просыпается мафия, у неё есть минута что бы договориться!");
        firstDict.Add(Messages.FIRST_NIGHT_MAFIA_END, "Мафия засыпает.");
        firstDict.Add(Messages.GAME_ALREADY_STARTED, "Игра уже начата");
        firstDict.Add(Messages.EMPTY_SLOT, "Пустой слот");
        firstDict.Add(Messages.ERROR_CHECK_PLAYER, "Проверьте правильность заполнения стола.");
        firstDict.Add(Messages.BOSS_CHEKING, "Мафия засыпает. Просыпается дон и ищет щерифа!");
        firstDict.Add(Messages.BOSS_CONFIRM, "Вы уверены что дон провил именно этого игрока?");
        firstDict.Add(Messages.MISS_BOSS_CHECK_CONFIRM, "Вы уверены что дон никого не проверил?");
        firstDict.Add(Messages.SHERIF_CHEKING, "Дон засыпает. Просыпается шериф и ищет мафию!");
        firstDict.Add(Messages.SHERIF_CONFIRM, "Вы уверены что шериф провил именно этого игрока?");
        firstDict.Add(Messages.MISS_SHERIF_CHECK_CONFIRM, "Вы уверены что шериф никого не проверил?");
        firstDict.Add(Messages.BEST_TURN, "У убитого игрока есть 20 секунд на лучший ход.");
        firstDict.Add(Messages.MAFIA_SHOOTING, "Мафия поднимает своё оружие и проходит мимо дома №.....");
        firstDict.Add(Messages.WARN_CONFIRM, "Вы уверены что хотите выдать предупреждение?");
        firstDict.Add(Messages.KICK_CONFIRM, "Вы уверены что хотите выгнать игрока?");
        firstDict.Add(Messages.MUTED_SPEAKER, ", вы не можете говорить, у вас 3 фола. Вы выставляете кого-нибудь на голосование?");
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
