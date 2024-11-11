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
    [SerializeField] private TMP_Text seocndsText;
    [SerializeField] private TMP_Text headText;
    public int currentSeconds;
    private GameState currentGameState;

    private void Start()
    {
        resetButton.onClick.AddListener(ResetTimer);
        bonusButton.onClick.AddListener(BonusTimer);
        nextButton.onClick.AddListener(NextState);
        //seocndsText.text = "реяр";
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
        if (sec > 0) nextButton.gameObject.SetActive(false);
        if (sec == 0) OnTimerEnd();
        if(sec < 0) sec = 0;
        currentSeconds = sec;

        seocndsText.SetText(currentSeconds.ToString());             
    }

    private void OnTimerEnd()
    {
        nextButton.gameObject.SetActive(true);
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
}
