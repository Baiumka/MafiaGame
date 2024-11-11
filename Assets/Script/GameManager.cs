using System;
using System.Timers;

public delegate void StartGameHandler(Game newGame); 
public delegate void ErrorHandler(string errorText);
public delegate void GameStateHandler(GameState gameState);
public delegate void LeftSecondsHandler(int now, int final);
public class GameManager 
{
    private Game currentGame;
    private Timer timer;
    private int secondsTicked;
    private int currentTimerFinal;

    private GameState gameState;
    public StartGameHandler onGameStarted;
    public ErrorHandler onGameManagerGotError;
    public GameStateHandler onGameStateChanged;
    public LeftSecondsHandler onTimerTicked;

    public GameState GameState { get => gameState; }

    public GameManager()
    {
        Translator.Init();
        timer = new Timer(1000);
        timer.Elapsed += OnTimerTick;
        timer.AutoReset = true;
    }

    public void CreateGame(int playerCount)
    {
        if(currentGame == null)
        {
            currentGame = new Game(playerCount);
            gameState = GameState.GIVE_ROLE;
            onGameStarted?.Invoke(currentGame);            
        }
        else
        {
            onGameManagerGotError?.Invoke(Translator.Message(Messages.GAME_ALREADY_STARTED));
        }
    }

    public void NextState()
    {
        switch (gameState)
        {
            case GameState.FIRST_NIGHT_MAFIA:
                SetState(GameState.FIRST_NIGHT_SHERIF);
                StartTimer(30);
                break;
            default:
                break;
        }

    }

    public void StartGame()
    {
        //if(currentGame.CheckPlayer())
        if(true)
        {
            SetState(GameState.FIRST_NIGHT_MAFIA);            
            StartTimer(60);            
        }
        else
        {
            onGameManagerGotError?.Invoke(Translator.Message(Messages.ERROR_CHECK_PLAYER));
        }
    }

    public void BonusTime(int seconds)
    {
        secondsTicked -= 10;
    }

    public void ResetTimer()
    {
        secondsTicked = 0;
    }

    private void SetState(GameState state)
    {
        gameState = state;
        onGameStateChanged?.Invoke(gameState);
    }

    private void StartTimer(int seconds)
    {     
        secondsTicked = 0;
        currentTimerFinal = 3;
        //currentTimerFinal = seconds;
        onTimerTicked?.Invoke(secondsTicked, currentTimerFinal);
        timer.Start();
    }

    private void OnTimerTick(object sender, ElapsedEventArgs e)
    {
        secondsTicked++;
        onTimerTicked?.Invoke(secondsTicked, currentTimerFinal);
        if (secondsTicked >= currentTimerFinal)
        {
            timer.Stop();          
            OnTimerCompleted();
        }
    }

    private void OnTimerCompleted()
    {
        switch(gameState)
        {
            case GameState.FIRST_NIGHT_MAFIA:
                //SetState(GameState.FIRST_NIGHT_SHERIF);
                //StartTimer(30);
                break;
            default:
                break;
        }
    }
}
