using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;


public class PHPDatabase : IDataBase
{
    private const string MAIN_URL = "http://95.169.201.208/";
    private const string LOGIN_PHP = MAIN_URL + "/login.php";
    private const string ADD_PEOPLE_PHP = MAIN_URL + "/add_people.php";
    private const string NEW_GAME = MAIN_URL + "/new_game.php";
    private const string ADD_HISTORY = MAIN_URL + "/add_history.php";
    private const string END_GAME = MAIN_URL + "/end_game.php";
    private const string GAME_LIST = MAIN_URL + "/game_list.php";
    private const string GAME_INFO = MAIN_URL + "/game_info.php";

    private bool isLogin;
    private User loggedUser;
    private int currentGameId;
    private Game currentGame;
    private List<People> avaiblePeople;
    public bool IsLogin => isLogin;
    public List<People> AvaiblePeople => avaiblePeople;

    public event UserInfoHandler OnUserLogin;
    public event ErrorHandler OnDataBaseError;
    public event PeopleInfoHandler OnPeopleAdded;
    public event GameListHandler OnGameListRefreshed;
    public event GameDetailsHandler OnGameDetailsUpdated;

    private HttpClient _httpClient;
    
    public PHPDatabase() 
    {
        _httpClient = new HttpClient();
    }

    public void AddPeople(string name)
    {
       AddCharAsync(name);
    }

    public async void RefreshGameList()
    {
        try
        {
            string fullUrl = $"{GAME_LIST}?id_user={loggedUser.id}";
            HttpResponseMessage answer = await _httpClient.GetAsync(fullUrl);
            answer.EnsureSuccessStatusCode();
            string responseBody = await answer.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<GameListResponse>(responseBody);
            if (response != null && response.status == "success")
            {
                OnGameListRefreshed?.Invoke(response.games.ToList());
            }
            else
            {
                OnDataBaseError?.Invoke($"Ошибка: {response?.message}");
            }

        }
        catch (Exception ex)
        {
            OnDataBaseError?.Invoke($"Error: {ex.Message}");
        }
    }

    public async void GetGameDetails(GameInfo game)
    {
        try
        {
            string fullUrl = $"{GAME_INFO}?id_game={game.id}";
            HttpResponseMessage answer = await _httpClient.GetAsync(fullUrl);
            answer.EnsureSuccessStatusCode();
            string responseBody = await answer.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<GameDetailsResponse>(responseBody);
            if (response != null && response.status == "success")
            {
                OnGameDetailsUpdated?.Invoke(response.players, response.log, game);
            }
            else
            {
                OnDataBaseError?.Invoke($"Ошибка: error on get details");
            }

        }
        catch (Exception ex)
        {
            OnDataBaseError?.Invoke($"Error: {ex.Message}");
        }
    }

    public async void AddCharAsync(string nickname)
    {
        try
        {
            // Подготовка данных в формате application/x-www-form-urlencoded
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("id_user", loggedUser.id.ToString()),
                new KeyValuePair<string, string>("name", nickname),
            });
            HttpResponseMessage response = await _httpClient.PostAsync(ADD_PEOPLE_PHP, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IntResponse>(responseBody);
            if (result != null && result.status == "success")
            {
                People newPeople = new People(result.id, nickname);
                avaiblePeople.Add(newPeople);
                OnPeopleAdded?.Invoke(newPeople);
            }
            else
            {
                OnDataBaseError?.Invoke($"Ошибка: {result?.message}");
            }
        }
        catch (Exception ex)
        {
            OnDataBaseError?.Invoke($"Ошибка запроса: {ex.Message}");
        }
    }


    public void Register(string login, string password, string name)
    {
        OnDataBaseError?.Invoke("Регистрация недоступна.");
    }

    public void Login(string login, string password)
    {
         LoginAsync(login, password);
    }

    public async void AddGameAsync(List<Player> players)
    {
        try
        {           
            var requestData = new { players = players, id_user = loggedUser.id };
            string jsonContent = JsonConvert.SerializeObject(requestData);            
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(NEW_GAME, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IntResponse>(responseBody);
            if (result != null && result.status == "success")
            {
                currentGameId = result.id;
                //OnDataBaseError?.Invoke($"Ошибка: {currentGameId}");
            }
            else
            {
                OnDataBaseError?.Invoke($"Ошибка: {result?.message}");
            }
        }
        catch (Exception ex)
        {
            OnDataBaseError?.Invoke($"Ошибка запроса: {ex.Message}");

        }
    }

    public async void LoginAsync(string login, string password)
    {

        try
        {
            // Формирование полного URL с параметрами
            string fullUrl = $"{LOGIN_PHP}?login={Uri.EscapeDataString(login)}&pass={Uri.EscapeDataString(password)}";

            // Отправка GET-запроса
            HttpResponseMessage answer = await _httpClient.GetAsync(fullUrl);

            // Проверка успешности ответа
            answer.EnsureSuccessStatusCode();

            // Получение текста ответа
            string responseBody = await answer.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<LoginResponse>(responseBody);

            if (response != null && response.status == "success")
            {
                SuccssesLogin(response);
            }
            else
            {
                OnDataBaseError?.Invoke($"Ошибка: {response?.message}");
            }

        }
        catch (Exception ex)
        {
            OnDataBaseError?.Invoke($"Error: {ex.Message}");
        }
    }

    private void SuccssesLogin(LoginResponse response)
    {
        loggedUser = response.user;
        isLogin = true;
        OnUserLogin?.Invoke(loggedUser.number, loggedUser.name);
        ReLoadPeopleList    (response.people);
        
    }

    private void ReLoadPeopleList(People[] people)
    {
        avaiblePeople = new List<People>();
        avaiblePeople.AddRange(people);
    }

    public void StartNewGame(Game game)
    {
        AddGameAsync(game.Players);
        currentGame = game;
        currentGame.history.onHistoryAdded += WriteHistory;
    }

    public async void WriteHistory(History.HistoryEvent historyEvent)
    {
        try
        {
            var requestData = new { history = historyEvent, id_game = currentGameId };
            string jsonContent = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(ADD_HISTORY, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IntResponse>(responseBody);
            if (result != null && result.status == "success")
            {
                //result.id;
            }
            else
            {
                OnDataBaseError?.Invoke($"Ошибка: {result?.message}");
            }
        }
        catch (Exception ex)
        {
            OnDataBaseError?.Invoke($"Ошибка запроса: {ex.Message}");

        }
    }

    public async void EndGame(Game game, int winner)
    {
        try
        {
            var requestData = new { id_game = currentGameId, players = game.Players, game_winner = winner, mafia = game.Mafia, citizen = game.Citizens };
            string jsonContent = JsonConvert.SerializeObject(requestData);
            //OnDataBaseError?.Invoke(jsonContent);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(END_GAME, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IntResponse>(responseBody);
            if (result != null && result.status == "success")
            {
                currentGameId = 0;
                currentGame.history.onHistoryAdded -= WriteHistory;
                currentGame = null;
            }
            else
            {
                OnDataBaseError?.Invoke($"Ошибка: {result?.message}");
            }
        }
        catch (Exception ex)
        {
            OnDataBaseError?.Invoke($"Ошибка запроса: {ex.Message}");

        }
    }

    internal static People GetRandomPlayer()
    {
        List<People> players = Controller.AvaiblePeople;
        if (players == null || players.Count == 0)
        {
            return null; // или выбросьте исключение, если список пуст
        }

        System.Random random = new System.Random();
        int randomIndex = random.Next(players.Count);
        return players[randomIndex];
    }

    class GameDetailsResponse
    {
        public string status;
        public List<ResultPlayer> players;
        public List<ResultHistoryEvent> log;
    }


    class LoginResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public User user { get; set; }
        public People[] people { get; set; }
    }

    class GameListResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public GameInfo[] games { get; set; }
    }

    public class IntResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public int id { get; set; }
    }



}
