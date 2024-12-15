using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void VoidHandler();
public class Controller : MonoBehaviour
{
    public static Controller singlton;
    [SerializeField] private InterfaceManager interfaceManager;
    [SerializeField] private VisualManager visualManager;

    private GameManager gameManager;
    private IDataBase database;
    
    public GameHandler onGameStarted;
    public ErrorHandler onError;
    public VoidHandler onWantedPlayerList;
    public VoidHandler onWantedGamesList;
    public VoidHandler onWantedStartWidnow;
    public VoidHandler onWantedGameWindow;
    public VoidHandler onBackPressed;
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
    public UserInfoHandler onUserLogin;
    public PeopleInfoHandler onPeopleAdded;
    public GameListHandler onGameListRefreshed;
    public GameDetailsHandler onGameDetailsUpdated;

    private Player playerSlotSelecting;
    private Player playerRoleSelecting;


    public int MaxPlayerCount { get => gameManager.MaxPlayerCount; }
    public static List<People> AvaiblePeople { get => singlton.database.AvaiblePeople; }

    private void Awake()
    {    
        if (singlton == null)
        {
            singlton = this;
        }
        else
        {
            Destroy(this);
        }        
    }

    public void Login(string login, string password)
    {
        database.Login(login, password);
    }


    public void NextState()
    {
        if (gameManager.GameState == GameState.SHOOTING)
        {
            InterfaceManager.dialog.ShowDialog(
            Translator.Message(Messages.MISS_SHOOTING_CONFIRM),
            () => gameManager.NextState(),
            null
            );
        }
        else if (gameManager.GameState == GameState.BOSS)
        {
            InterfaceManager.dialog.ShowDialog(
            Translator.Message(Messages.MISS_BOSS_CHECK_CONFIRM),
            () => gameManager.NextState(),
            null
            );
        }
        else if (gameManager.GameState == GameState.SHERIF)
        {
            InterfaceManager.dialog.ShowDialog(
            Translator.Message(Messages.MISS_SHERIF_CHECK_CONFIRM),
            () => gameManager.NextState(),
            null
            );
        }
        else
        {
            gameManager.NextState();
        }
    }

    internal void WarnPlayer(Player playerInfo)
    {
        InterfaceManager.dialog.ShowDialog(
            Translator.Message(Messages.WARN_CONFIRM),
            () => gameManager.WarnPlayer(playerInfo),
            null
            );
    }

    internal void KickPlayer(Player playerInfo)
    {
        InterfaceManager.dialog.ShowDialog(
            Translator.Message(Messages.KICK_CONFIRM),
            () => gameManager.KickPlayer(playerInfo),
            null
            );
    }

    private void Start()
    {
        if(gameManager == null)
        {
            gameManager = new GameManager();
            database = new PHPDatabase();
            gameManager.onGameManagerGotError += ShowError;
            gameManager.onGameStarted += StartGame;
            gameManager.onTimerTicked += TickTimer;
            gameManager.onGameStateChanged += StateChange;
            gameManager.onCameNewDay += NewDay;
            gameManager.onNewSpeaker += ChangeSpeaker;
            gameManager.onVoteOfficial += VoteOfficial;
            gameManager.onPlayerVotedTurn += VotedTurn;
            gameManager.onVotesChanged += UpdateVotes;
            gameManager.onDopSpeakOfficial += DopSpeakOfficial;
            gameManager.onDopSpeakStarted += DopSpeakStarted;
            gameManager.onDopVoteOfficial += DopVoteOfficial;
            gameManager.onNightStarted += StartNight;
            gameManager.onLastWordStarted += StartLastWord;
            gameManager.onMafiaWin += MafiaWin;
            gameManager.onCitizenWin += CitizenWin;
            gameManager.onNoWin += NoWin;
            gameManager.onGameLaunched += LaunchGame;

            database.OnUserLogin += ShowUserInfo;
            database.OnDataBaseError += ShowError;
            database.OnPeopleAdded += AddPeople;
            database.OnGameListRefreshed += RefreshGameList;
            database.OnGameDetailsUpdated += ShowGameDetails;
        }
        interfaceManager.Init();
        visualManager.Init();


    }

    private void ShowGameDetails(List<ResultPlayer> players, List<ResultHistoryEvent> history, GameInfo game)
    {
        onGameDetailsUpdated?.Invoke(players, history, game);
    }

    private void RefreshGameList(List<GameInfo> gameList)
    {
        onGameListRefreshed?.Invoke(gameList);
    }

    private void LaunchGame(Game newGame)
    {
        database.StartNewGame(newGame); 
    }

    private void AddPeople(People people)
    {
        onPeopleAdded?.Invoke(people);
    }

    private void ShowUserInfo(int number, string name)
    {
        onUserLogin?.Invoke(number, name);
    }

    public void ResetTime()
    {
        gameManager.ResetTimer();
    }

    public void BonusTime(int seconds)
    {
        gameManager.BonusTime(seconds);
    }

    public void PutToVote(Player voted)
    {
        gameManager.PutToVote(voted);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnBack();            
        }
    }



    private void ReturnBack()
    {
        if(InterfaceManager.dialog.IsShown)
        {
            InterfaceManager.dialog.CloseDialog();
            return;
        }

        switch (gameManager.GameState)
        {
            case GameState.NONE:
                onWantedStartWidnow?.Invoke();
                playerSlotSelecting = null;
                break;
            case GameState.GIVE_ROLE:
                if (playerSlotSelecting != null)
                {
                    onWantedGameWindow?.Invoke();
                    playerSlotSelecting = null;
                }
                else
                {
                    onBackPressed?.Invoke();
                    if (playerRoleSelecting != null)
                    {
                        playerRoleSelecting = null;
                    }
                }
                break;
        }
    }

    public void PrepareForRole(Player player)
    {
        playerRoleSelecting = player;
    }

    public void PeopleClick(People peopleInfo)
    {
        if(gameManager.GameState == GameState.GIVE_ROLE)
        {
            if(playerSlotSelecting != null)
            {
                playerSlotSelecting.SetPeople(peopleInfo);
                ReturnBack();

            }
        }
        else
        {
            Debug.Log(peopleInfo.name);
        }
    }

    public void SelectRole(Role role)
    {
        if (playerRoleSelecting != null)
        {
            playerRoleSelecting.SetRole(role);
            playerRoleSelecting = null;
            onBackPressed?.Invoke();
        }
        else
        {
            onError?.Invoke("ERROR_NO_PLAYER_FOR_ROLE");
        }
    }

    public void SelectPeopleForSlot(Player player)
    {
        if (gameManager.GameState == GameState.GIVE_ROLE)
        {
            onWantedPlayerList?.Invoke();
            playerSlotSelecting = player;
        }
        else if (gameManager.GameState == GameState.VOTE || gameManager.GameState == GameState.DOP_VOTE || gameManager.GameState == GameState.VOTE_FOR_UP)
        {
            if (player.Voted == null)
            {
                gameManager.Vote(player);
            }
            else
            {
                gameManager.TryUnVote(player);
            }
        }
        else if (gameManager.GameState == GameState.SHOOTING)
        {
            InterfaceManager.dialog.ShowDialog(
            Translator.Message(Messages.SHOOTING_CONFIRM) + player.Number + ". " + player.People.name,
            () => gameManager.ShotPlayer(player),
            null
            );
        }
        else if (gameManager.GameState == GameState.BOSS)
        {
            InterfaceManager.dialog.ShowDialog(
            Translator.Message(Messages.BOSS_CONFIRM) + player.Number + ". " + player.People.name,
            () => gameManager.BossCheckPlayer(player),
            null
            );
        }
        else if (gameManager.GameState == GameState.SHERIF)
        {
            InterfaceManager.dialog.ShowDialog(
            Translator.Message(Messages.SHERIF_CONFIRM) + player.Number + ". " + player.People.name,
            () => gameManager.SherifCheckPlayer(player),
            null
            );
        }
        else if (gameManager.GameState == GameState.BEST_TURN)
        {
            gameManager.AddBestTurn(player);
        }
    }

    public void ShowPlayerList()
    {
        onWantedPlayerList?.Invoke();
    }

    public void CreatePeople(string name)
    {
        if (name.Length < 2)
        {
            onError?.Invoke(Translator.Message(Messages.TOO_SHORT_NAME));
            return;
        }
        database.AddPeople(name);
    }
    public void CreateGame(int playerCount)
    {
        if (database.IsLogin)
            gameManager.CreateGame(playerCount);
        else
            onError?.Invoke(Translator.Message(Messages.NO_LOGIN));
    }

    public void StartGame()
    {
        gameManager.StartGame();
    }

    #region Events Duplicates

    private void NoWin(Game game)
    {
        onNoWin?.Invoke(game);
        //database.WriteResult(game);
        database.EndGame(game, 0);
    }

    private void CitizenWin(Game game)
    {
        onCitizenWin?.Invoke(game);
        //database.WriteResult(game);
        database.EndGame(game, 1);
    }

    private void MafiaWin(Game game)
    {
        onMafiaWin?.Invoke(game);
        //database.WriteResult(game);
        database.EndGame(game, 2);
    }
    private void StartLastWord(Player player)
    {
        onLastWordStarted?.Invoke(player);
        
    }

    private void StartNight()
    {
        onNightStarted?.Invoke();
    }
    private void StateChange(GameState gameState)
    {    
        onGameStateChanged?.Invoke(gameState);
    }
    private void VotedTurn(Player player)
    {
        onPlayerVotedTurn?.Invoke(player);
    }

    private void TickTimer(int now, int final)
    {
        onTimerTicked?.Invoke(now, final);
    }
    private void ShowError(string errorText)
    {
        onError?.Invoke(errorText);
    }

    private void UpdateVotes(int i)
    {
        onVotesChanged?.Invoke(i);
    }
    private void StartGame(Game newGame)
    {
        onGameStarted?.Invoke(newGame);
    }

    private void ChangeSpeaker(Player player)
    {
        onNewSpeaker?.Invoke(player);
    }

    private void NewDay(int dayNumber)
    {
        onCameNewDay?.Invoke(dayNumber);
    }
    private void VoteOfficial(List<Player> votedPlayers)
    {
        onVoteOfficial?.Invoke(votedPlayers);
    }
    private void DopVoteOfficial(List<Player> votedPlayers)
    {
        onDopVoteOfficial?.Invoke(votedPlayers);
    }

    private void DopSpeakStarted(Player player)
    {
        onDopSpeakStarted?.Invoke(player);
    }

    private void DopSpeakOfficial(List<Player> votedPlayers)
    {
        onDopSpeakOfficial?.Invoke(votedPlayers);
    }

    internal void Register(string text1, string text2, string text3)
    {
        onError?.Invoke("Регистрация недоступна");
    }

    internal void ShowGamesList()
    {
        database.RefreshGameList();
        onWantedGamesList?.Invoke();
    }

    internal void ShowFullGameInfo(GameInfo gameInfo)
    {
        database.GetGameDetails(gameInfo);
    }

    #endregion
}
