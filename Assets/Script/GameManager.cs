using System;
using System.Collections.Generic;
using System.Timers;

public delegate void GameHandler(Game newGame); 
public delegate void ErrorHandler(string errorText);
public delegate void GameStateHandler(GameState gameState);
public delegate void LeftSecondsHandler(int now, int final);
public delegate void DayHandler(int dayNumber);
public delegate void PlayerHandler(Player player);
public delegate void VoteOfficialHandler(List<Player> votedPlayers);
public delegate void IntHandler(int i);
public class GameManager 
{
    private Game currentGame;
    private Timer timer;
    private int secondsTicked;
    private int currentTimerFinal;
    private int currentTurn;
    private Player firstSpeaker;
    private Player speakPlayer;
    private bool isPutted;
    private List<Player> votedPlayers = new List<Player>();
    private Dictionary<Player, int> voteResult;
    private int votePlayerIndex;
    private int dopSpkeakIndex;
    private Player shootedPlayer;
    private Player firstKilled;
    private List<Player> bestTurn;
    //private int firstSpeakerIndex;

    private GameState gameState;
    public GameHandler onGameStarted;
    public ErrorHandler onGameManagerGotError;
    public GameStateHandler onGameStateChanged;
    public LeftSecondsHandler onTimerTicked;
    public DayHandler onCameNewDay;
    public PlayerHandler onNewSpeaker;
    public PlayerHandler onPlayerVotedTurn;  
    public VoteOfficialHandler onVoteOfficial;
    public IntHandler onVotesChanged;
    public VoteOfficialHandler onDopSpeakOfficial;
    public VoteOfficialHandler onDopVoteOfficial;
    public PlayerHandler onDopSpeakStarted;
    public PlayerHandler onLastWordStarted;
    public VoidHandler onNightStarted;
    public GameHandler onMafiaWin;
    public GameHandler onCitizenWin;
    public GameHandler onNoWin;
    public GameState GameState { get => gameState; }
    public int MaxPlayerCount { get => currentGame.Players.Count; }

    public GameManager()
    {
        Translator.Init();
        timer = new Timer(1000);
        timer.Elapsed += OnTimerTick;
        timer.AutoReset = true;
    }

    public void PutToVote(Player voted)
    {
        if(voted.IsPutted)
        {
            onGameManagerGotError?.Invoke(Translator.Message(Messages.ALREADY_VOTED));
            return;
        }
        if(isPutted)
        {
            onGameManagerGotError?.Invoke(Translator.Message(Messages.ALREADY_PUTTED));
            return;
        }
        voted.Put();
        votedPlayers.Add(voted);
        currentGame.Log(speakPlayer,voted,EventType.PUT);
        isPutted = true;
        
    }

    internal void WarnPlayer(Player player)
    {
        player.GiveWarn();
        currentGame.Log(null, player, EventType.WARN);
        if (player.Warn >= 4)
        {
            currentGame.Log(null, player, EventType.WARN_EXIT);
            KickPlayer(player);        
        }
    }

    internal void KickPlayer(Player player)
    {
        player.Die();
        CheckWinner();
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

    private void CheckWinner()
    {
        if(currentGame.Mafia >= currentGame.Citizens && currentGame.Mafia > 0)
        {
            onMafiaWin?.Invoke(currentGame);            
        }

        if(currentGame.Mafia == 0)
        {
            if(currentGame.Citizens >= 0)
            {
                onCitizenWin?.Invoke(currentGame);
            }
            else
            {
                //onGameManagerGotError?.Invoke("Мафия: " + currentGame.Mafia + " Alive: " + currentGame.Citizens);
                onNoWin?.Invoke(currentGame);
            }            
        }
        SetState(GameState.CLOSING);
        timer.Stop();
    }
   

    private List<Player> CountVoted()
    {
        int max = 0;
        List<Player> maxsPlayers = new List<Player>();
        foreach (Player p in voteResult.Keys)
        {
            if (voteResult[p] > max)
            {
                max = voteResult[p];
                maxsPlayers.Clear();
                maxsPlayers.Add(p);
                continue;
            }
            if (voteResult[p] == max && max > 0)
            {
                maxsPlayers.Add(p);
                continue;
            }
        }
        foreach (Player p in currentGame.Players)
        {
            p.UnVote();
        }
        if (maxsPlayers.Count == 1)
        {
            GiveLastWord(maxsPlayers[0]);
            return null;
        }
        else
        {
            return maxsPlayers;
        }
    }
        


    public void NextState()
    {
        switch (gameState)
        {
            case GameState.BEST_TURN:
                SetState(GameState.MORNING);
                break;
            case GameState.SHERIF:
                if (currentTurn == 1 && shootedPlayer != null)
                {
                    firstKilled = shootedPlayer;
                    bestTurn = new List<Player>();
                    SetState(GameState.BEST_TURN);
                    StartTimer(20);
                }
                else
                {
                    SetState(GameState.MORNING);                    
                }
                
                break;
            case GameState.BOSS:
                SetState(GameState.SHERIF);
                StartTimer(30);
                //Добавляем лог
                break;
            case GameState.SHOOTING:
                shootedPlayer = null;
                SetState(GameState.BOSS);
                StartTimer(30);
                //Добавляем лог
                break;
            case GameState.NIGHT:
                shootedPlayer = null;
                SetState(GameState.SHOOTING);
                break;
            case GameState.DOP_VOTE_LAST_WORD:
                votePlayerIndex++;
                if(votePlayerIndex < votedPlayers.Count)
                {
                    GiveLastWord(votedPlayers[votePlayerIndex]);
                }
                else
                {
                    CheckWinner();
                    StartNight();
                }
                break;
            case GameState.VOTE_LAST_WORD:
                CheckWinner();
                StartNight();
                break;
            case GameState.VOTE_FOR_UP:
                float result = voteResult[votedPlayers[votePlayerIndex]];
                float alivePlayersCount = currentGame.AlivePlayers.Count / 2;
                if (result > alivePlayersCount)
                {
                    votePlayerIndex = 0;
                    GiveLastWord(votedPlayers[votePlayerIndex]);
                }
                else
                {
                    StartNight();
                }
                break;
            case GameState.DOP_VOTE:
                if (votePlayerIndex == votedPlayers.Count - 1)
                {
                    foreach (Player p in currentGame.AlivePlayers)
                    {
                        if (p.Voted == null) Vote(p);
                    }
                }
                votePlayerIndex++;
                if (votePlayerIndex >= votedPlayers.Count)
                {
                    List<Player> results = CountVoted();
                    if (results != null)
                    {
                        votePlayerIndex--;
                        voteResult[votedPlayers[votePlayerIndex]] = 0;
                        SetState(GameState.VOTE_FOR_UP);
                    }
                }
                else
                {
                    onPlayerVotedTurn?.Invoke(votedPlayers[votePlayerIndex]);
                }
                break;
            case GameState.DOP_VOTE_OFFICIAL:
                votePlayerIndex=0;
                voteResult = new Dictionary<Player, int>();
                foreach (Player p in votedPlayers)
                    voteResult[p] = 0;
                SetState(GameState.DOP_VOTE);
                onPlayerVotedTurn?.Invoke(votedPlayers[votePlayerIndex]);                
                break;
            case GameState.DOP_SPEAK:
                dopSpkeakIndex++;
                if (dopSpkeakIndex >= votedPlayers.Count)
                {
                    timer.Stop();
                    onDopVoteOfficial?.Invoke(votedPlayers);
                    SetState(GameState.DOP_VOTE_OFFICIAL);
                    
                }
                else
                {                    
                    speakPlayer = votedPlayers[dopSpkeakIndex];
                    onDopSpeakStarted?.Invoke(speakPlayer);
                    StartTimer(30);
                }
                break;
            case GameState.VOTE:
                if (votePlayerIndex == votedPlayers.Count - 1)
                {
                    foreach (Player p in currentGame.AlivePlayers)
                    {
                        if (p.Voted == null) Vote(p);
                    }
                }
                votePlayerIndex++;                
                if (votePlayerIndex >= votedPlayers.Count)
                {
                    List<Player> results = CountVoted();
                    if (results != null)                    
                    {
                        dopSpkeakIndex = -1;
                        votedPlayers = results;
                        onDopSpeakOfficial?.Invoke(results);
                        SetState(GameState.DOP_SPEAK);
                    }
                }
                else
                {
                    onPlayerVotedTurn?.Invoke(votedPlayers[votePlayerIndex]);
                }
                break;
            case GameState.VOTE_OFFIACIAL:
                votePlayerIndex = 0;
                
                if(votedPlayers.Count == 0)
                {
                    StartNight();
                }
                else if(votedPlayers.Count == 1)
                {
                    GiveLastWord(votedPlayers[0]);
                }
                else
                {
                    voteResult = new Dictionary<Player, int>();
                    foreach (Player p in votedPlayers)
                        voteResult[p] = 0;
                    SetState(GameState.VOTE);
                    onPlayerVotedTurn?.Invoke(votedPlayers[votePlayerIndex]);
                }    
                break;
            case GameState.SHOT_LAST_WORD:
                CheckWinner();
                StandartMorning();
                break;
            case GameState.DAY:
                isPutted = false;
                Player nextPlayer = currentGame.GetNextPlayer(speakPlayer);
                if (nextPlayer == firstSpeaker)
                {
                    timer.Stop();
                    SetState(GameState.VOTE_OFFIACIAL);
                    onVoteOfficial?.Invoke(votedPlayers);
                }
                else
                {
                    speakPlayer = nextPlayer;
                    if (speakPlayer.Warn >= 3)
                    {
                        timer.Stop();
                    }
                    else
                    {
                        StartTimer(60);
                        
                    }
                    onNewSpeaker?.Invoke(speakPlayer);
                }
                break;
            case GameState.MORNING:
                if(shootedPlayer != null)
                {
                    GiveLastWord(shootedPlayer);
                    shootedPlayer = null;
                    break;
                }
                StandartMorning();


                break;
            case GameState.FIRST_NIGHT_SHERIF:                
                SetState(GameState.MORNING);
                break;
            case GameState.FIRST_NIGHT_MAFIA:
                SetState(GameState.FIRST_NIGHT_SHERIF);
                StartTimer(30);                
                break;
            default:
                break;
        }

    }

    private void StandartMorning()
    {
        foreach (Player player in currentGame.Players)
        {
            player.UnPut();
            player.RemoveBestTurn();
        }
        votedPlayers.Clear();
        currentTurn++;
        onCameNewDay?.Invoke(currentTurn);
        speakPlayer = currentGame.GetNextPlayer(firstSpeaker);
        firstSpeaker = speakPlayer;
        onNewSpeaker?.Invoke(speakPlayer);
        SetState(GameState.DAY);
        StartTimer(60);
    }

    public void AddBestTurn(Player player)
    {
        if(bestTurn != null)
        {
            if(bestTurn.Contains(player))
            {
                bestTurn.Remove(player);
                player.RemoveBestTurn();
            }
            else
            {
                if (bestTurn.Count < 3)
                {
                    bestTurn.Add(player);
                    player.AddBestTurn();
                    currentGame.Log(firstKilled, player, EventType.BEST_TURN);
                }
            }
            
        }
    }

    public void SherifCheckPlayer(Player player)
    {
        currentGame.Log(currentGame.Sherif, player, EventType.SHERIF_CHECK);
        NextState();
        //Добавляем лог
        
    }

    public void BossCheckPlayer(Player player)
    {
        currentGame.Log(currentGame.Boss, player, EventType.BOSS_CHECK);
        NextState();
        //Добавляем лог
    }

    public void ShotPlayer(Player player)
    {
        NextState();
        shootedPlayer = player;
        currentGame.Log(null, player, EventType.KILL);
        player.PrepareForDie();
        //Добавляем лог
    }

    private void StartNight()
    {
        foreach (Player p in currentGame.Players)
        {
            p.UnVote();
        }       
        SetState(GameState.NIGHT);
    }

    private void GiveLastWord(Player player)
    {
        if (gameState == GameState.VOTE_FOR_UP)
        {
            SetState(GameState.DOP_VOTE_LAST_WORD);
        }
        else if(gameState == GameState.MORNING)
        {
            SetState(GameState.SHOT_LAST_WORD);
        }
        else
        {
            SetState(GameState.VOTE_LAST_WORD);
        }
        player.Die();
        onLastWordStarted?.Invoke(player);
        StartTimer(60);
        
    }

    public void Vote(Player player)
    {
        if (player.Voted != null)
        {
            onGameManagerGotError?.Invoke(Translator.Message(Messages.ALREADY_VOTED_VOTE));
            return;
        }
        if(player.IsDead)
        {
            onGameManagerGotError?.Invoke(Translator.Message(Messages.ALREADY_DEAD));
            return;
        }

        player.Vote(votedPlayers[votePlayerIndex]);
        voteResult[votedPlayers[votePlayerIndex]]++;
        onVotesChanged?.Invoke(voteResult[votedPlayers[votePlayerIndex]]);        
    }

    public void TryUnVote(Player player)
    {
        if (player.Voted == votedPlayers[votePlayerIndex])
        {
            player.UnVote();
            voteResult[votedPlayers[votePlayerIndex]]--;
            onVotesChanged?.Invoke(voteResult[votedPlayers[votePlayerIndex]]);
        }
        else
        {
            onGameManagerGotError?.Invoke(Translator.Message(Messages.TOO_LATE_UNVOTE));
            return;
        }
    }

    public void StartGame()
    {
        if(currentGame.CheckPlayer())
        //if(true)
        {
            currentTurn = 0;
            isPutted = false;
            votedPlayers = new List<Player>();
            voteResult = null;
            votePlayerIndex = 0;
            dopSpkeakIndex = 0;
            shootedPlayer = null;
            firstKilled = null;
            bestTurn = null;
            firstSpeaker = null;
            currentTurn = 0;
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
        currentTimerFinal = seconds;        
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
                break;
            default:
                break;
        }
    }
}
