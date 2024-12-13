using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public class PHPDatabase : IDataBase
{
    private const string MAIN_URL = "http://95.169.201.208/";
    private const string LOGIN_PHP = MAIN_URL + "/login.php";

    private bool isLogin;
    private User loggedUser;
    private List<People> avaiblePeople;
    public bool IsLogin => isLogin;
    public List<People> AvaiblePeople => avaiblePeople;

    public event UserInfoHandler OnUserLogin;
    public event ErrorHandler OnDataBaseError;
    public event PeopleInfoHandler OnPeopleAdded;

    public void AddPeople(string name)
    {
        throw new System.NotImplementedException();
    }

    public void Register(string login, string password, string name)
    {
        throw new System.NotImplementedException();
    }

    public void WriteResult(Game game)
    {
        throw new System.NotImplementedException();
    }

    public async void Login(string login, string password)
    {
        await LoginAsync(login, password);
    }

    public async Task LoginAsync(string login, string password)
    {

        try
        {
            HttpClient _httpClient = new HttpClient();
            // Формирование полного URL с параметрами
            string fullUrl = $"{LOGIN_PHP}?login={Uri.EscapeDataString(login)}&pass={Uri.EscapeDataString(password)}";

            // Отправка GET-запроса
            HttpResponseMessage answer = await _httpClient.GetAsync(fullUrl);

            // Проверка успешности ответа
            answer.EnsureSuccessStatusCode();

            // Получение текста ответа
            string responseBody = await answer.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<ServerResponse>(responseBody);

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

    private void SuccssesLogin(ServerResponse response)
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

    class ServerResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public User user { get; set; }
        public People[] people { get; set; }
    }

    

}
