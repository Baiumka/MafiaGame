using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Timers;


public class TickPanel : MonoBehaviour
{
    [SerializeField] private Button resetButton;
    [SerializeField] private Button bonusButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Text seocndsText;
    [SerializeField] private TMP_Text headText;
    private int currentSeconds;
    private GameState currentGameState;
    private Timer timer;
    private int timerTicks;
    private int timerTicked = -1;
    private int timerHouse = -1;

    public void Init()
    {
        resetButton.onClick.AddListener(ResetTimer);
        bonusButton.onClick.AddListener(BonusTimer);
        nextButton.onClick.AddListener(NextState);
        seocndsText.color = Color.white;
        seocndsText.text = "";

        timer = new Timer(1500);
        timer.Elapsed += OnTimerTick;
        timer.AutoReset = true;
    }

    private void OnTimerTick(object sender, ElapsedEventArgs e)
    {
        if (timerTicks < Controller.singlton.MaxPlayerCount)
        {
            timerTicks++;
            timerHouse = timerTicks;            
        }
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

    private void Update()
    {        
        if (timerTicked != -1)
        {                      
            if (timerTicked == 0) OnTimerEnd();
            if (timerTicked == 10) AudioManager.instance.Play(AudioSounds.TEN_SECONDS);
            if (timerTicked < 0) timerTicked = 0;
            currentSeconds = timerTicked;
            seocndsText.color = Color.white;
            seocndsText.text = currentSeconds.ToString();
            timerTicked = -1;
        }
        if(timerHouse != -1)
        {
            seocndsText.color = Color.black;
            seocndsText.text = timerTicks.ToString();
            timerHouse = -1;
        }
    }

    public void SetSeconds(int sec)
    {
        timerTicked = sec; 
    }

    public void SetSpeaker(Player player)
    {
        Clean();        
        if (player.Warn >= 3)
        {
            headText.text = player.Number + " " + player.People.name + " " + Translator.Message(Messages.MUTED_SPEAKER);
        }
        else
        {
            headText.text = Translator.Message(Messages.SPEAKER) + "\n" + player.Number + " " + player.People.name;
        }
    }

    public void SetVoteCount(int i)
    {
        seocndsText.color = Color.red;
        seocndsText.text = Translator.Message(Messages.VOTES_COUNT) + " " + i;
    }

    public void VotePlayer(Player player)
    {
        headText.text = Translator.Message(Messages.VOTE) + player.Number + "?";
        seocndsText.text = "";
    }

    public void LastWord(Player player)
    {
        Clean();
        headText.text = player.People.name + " " + Translator.Message(Messages.YOUR_LAST_WORD);
    }

    public void DopVoteOfficial(List<Player> votedPlayers)
    {
        VoteOfficial(votedPlayers);
    }


    public void VoteOfficial(List<Player> votedPlayers)
    {
        string playersText = "\n";
        int i = 1;
        /*foreach(Player player in votedPlayers)
        {
            playersText += "<color=#"+ (
                    (player.Role == Role.CITIZEN || player.Role == Role.SHERIFF) ? 
                    ColorUtility.ToHtmlStringRGB(ColorStore.store.CITIZEN_BACKGROUND_COLOR) : 
                    ColorUtility.ToHtmlStringRGB(ColorStore.store.MAFIA_BACKGROUND_COLOR)
                    ) 
                + ">" + player.Number + "</color>" + 
                (i == votedPlayers.Count ? "<color=\"white\">. </color>" : "<color=\"white\">, </color>");
            i++;
        }*/
        foreach (Player player in votedPlayers)
        {
            playersText += player.Number.ToString() + (i == votedPlayers.Count ? "." : ", ");
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
        switch (gameState)
        {
            case GameState.BEST_TURN:
                headText.text = Translator.Message(Messages.BEST_TURN);
                break;
            case GameState.SHERIF:
                headText.text = Translator.Message(Messages.SHERIF_CHEKING);
                break;
            case GameState.BOSS:
                Clean();
                headText.text = Translator.Message(Messages.BOSS_CHEKING);
                timerTicks = 0;
                timer.Stop();
                break;
            case GameState.SHOOTING:
                headText.text = Translator.Message(Messages.MAFIA_SHOOTING);
                timerTicks = 0;
                timer.Start();
                break;
            case GameState.NIGHT:
                Clean();
                headText.text = Translator.Message(Messages.START_NIGHT);
                break;
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
