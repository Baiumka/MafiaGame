using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TimerPanel : MonoBehaviour
{
    [SerializeField] private Button resetButton;
    [SerializeField] private Button bonusButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Text seocndsText;
    [SerializeField] private TMP_Text headText;
    public int currentSeconds;
    private GameState currentGameState;

    private void Start()
    {
        resetButton.onClick.AddListener(ResetTimer);
        bonusButton.onClick.AddListener(BonusTimer);
        nextButton.onClick.AddListener(NextState);
        seocndsText.color = Color.white;
        seocndsText.text = "";
    }

    private void NextState()
    {
        Controller.singlton.NextState();
    }

    private void BonusTimer()
    {
        Controller.singlton.BonusTime(10);
    }

    private void ResetTimer()
    {
        Controller.singlton.ResetTime();
    }

    public void SetSeconds(int sec)
    {
        //if (sec > 0) nextButton.gameObject.SetActive(false);
        if (sec == 0) OnTimerEnd();
        if(sec < 0) sec = 0;
        currentSeconds = sec;

        seocndsText.text = currentSeconds.ToString();             
    }

    public void SetSpeaker(Player player)
    {
        headText.text = Translator.Message(Messages.SPEAKER) + player.Number + " " + player.People.Nickname;
    }

    public void SetVoteCount(int i)
    {
        seocndsText.color = Color.red;
        seocndsText.text =  Translator.Message(Messages.VOTES_COUNT) +  " " + i;
    }

    public void VotePlayer(Player player)
    {
        headText.text = Translator.Message(Messages.VOTE) + player.Number + "?";
        seocndsText.text = "";
    }

    public void LastWord(Player player)
    {
        Clean();
        headText.text = player.People.Nickname + " " + Translator.Message(Messages.YOUR_LAST_WORD);
    }

    public void DopVoteOfficial(List<Player> votedPlayers)
    {
        VoteOfficial(votedPlayers);
    }

    public void StartNight()
    {
        Clean();
        headText.text = Translator.Message(Messages.START_NIGHT);
    }

    public void VoteOfficial(List<Player> votedPlayers)
    {
        string playersText = "\n";
        int i = 1;
        foreach(Player player in votedPlayers)
        {
            playersText += "<color=#"+ (
                    (player.Role == Role.CITIZEN || player.Role == Role.SHERIFF) ? 
                    ColorUtility.ToHtmlStringRGB(ColorStore.store.CITIZEN_BACKGROUND_COLOR) : 
                    ColorUtility.ToHtmlStringRGB(ColorStore.store.MAFIA_BACKGROUND_COLOR)
                    ) 
                + ">" + player.Number + "</color>" + 
                (i == votedPlayers.Count ? "<color=\"white\">. </color>" : "<color=\"white\">, </color>");
            i++;
        }
        playersText += "\n";
        headText.text = Translator.Message(Messages.VOTE_OFFICIAL1) + playersText + Translator.Message(Messages.VOTE_OFFICIAL2) + playersText + "";
    }

    public void DopSpeakOfficial(List<Player> votedPlayers)
    {
        string playersText = "\n";
        int i = 1;
        foreach (Player player in votedPlayers)
        {
            playersText += player.Number + (i == votedPlayers.Count ? ". " : ", ");
            i++;
        }
        playersText += "\n";
        headText.text = Translator.Message(Messages.DOP_SPEAK_OFFICIAL1) + playersText + " " + Translator.Message(Messages.DOP_SPEAK_OFFICIAL2);
    }

    public void VoteForUp()
    {
        Clean();
        headText.text = Translator.Message(Messages.VOTE_FOR_UP);
    }

    private void OnTimerEnd()
    {
        //nextButton.gameObject.SetActive(true);
        switch (currentGameState)
        {
            case GameState.FIRST_NIGHT_MAFIA:
                headText.text = Translator.Message(Messages.FIRST_NIGHT_MAFIA_END);
                break;
            case GameState.FIRST_NIGHT_SHERIF:
                headText.text = Translator.Message(Messages.FIRST_NIGHT_SHERIF_END);
                break;
            default:
                break;
        }
    }

    public void SetState(GameState gameState)
    {
        currentGameState = gameState;
        switch(gameState)
        {
            case GameState.FIRST_NIGHT_MAFIA:
                headText.text = Translator.Message(Messages.FIRST_NIGHT_MAFIA);
                break;
            case GameState.FIRST_NIGHT_SHERIF:
                headText.text = Translator.Message(Messages.FIRST_NIGHT_SHERIF);
                break;
            default:
                break;
        }
    }

    public void Clean()
    {
        seocndsText.text = "";
        headText.SetText("");
    }
}
