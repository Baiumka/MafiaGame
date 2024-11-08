using System;

public delegate void StartGameHandler(Game newGame); 
public delegate void ErrorHandler(string errorText); 
public class GameManager 
{
    private Game currentGame;
    private GameState gameState;


    public StartGameHandler onGameStarted;
    public ErrorHandler onGameManagerGotError;

    public GameState GameState { get => gameState; }

    public GameManager()
    {
        Translator.Init();
    }

    public void StartGame(int playerCount)
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

    public void SetPlayer(Player player, People people)
    {
        if(gameState == GameState.GIVE_ROLE)
        {
            currentGame.SetPlayer(player, people);
        }
    }
}
