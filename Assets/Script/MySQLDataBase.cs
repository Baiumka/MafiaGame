

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;


public class MySQLDataBase : IDataBase
{
    private const string CONNECTION_STRING = "Server=junction.proxy.rlwy.net;Port=43851;Database=railway;User ID=root;Password=XEhtSYcfTgVtnbpOOyUbJXSjVSukSEdo;";
    private MySqlConnection connection;
    private bool isLogin;
    private int userID = 0;
    private List<People> peopleList;
    public event UserInfoHandler OnUserLogin;
    public event PeopleInfoHandler OnPeopleAdded;
    public event ErrorHandler OnDataBaseError;
    public event GameListHandler OnGameListRefreshed;
    public event GameDetailsHandler OnGameDetailsUpdated;

    private MySqlConnection Conn 
    {
        get 
        {
            if (connection == null)
                return Connect();
            else if (!connection.Ping())
                return Connect();
            else
                return connection;
        }
    }
    public List<People> AvaiblePeople => peopleList;
    public bool IsLogin => isLogin;//Заменить на isLogin

    public MySQLDataBase()
    {
        peopleList = new List<People>();
    }

    private void LoadPeopleList()
    {
        try
        {
            string query = "SELECT id, name FROM people WHERE id_user = " + userID;
            //OnDataBaseError?.Invoke(query);             
            using (MySqlCommand cmd = new MySqlCommand(query, Conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Читаем данные из таблицы
                        int id = reader.GetInt32("id");
                        string name = reader.GetString("name");
                        //OnDataBaseError?.Invoke(name);
                        peopleList.Add(new People(id, name));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            OnDataBaseError?.Invoke($"Ошибка выполнения запроса: {ex.Message}");
            if (ex.InnerException != null)
            {
                OnDataBaseError?.Invoke($"Вложенная ошибка: {ex.InnerException.Message}");
            }
        }
    }

    private MySqlConnection Connect()
    {
        try
        {
            connection = new MySqlConnection(CONNECTION_STRING);
            connection.Open();

            /*string queryInsertPlayer = "DELETE FROM people";
            using (MySqlCommand cmdInsertPlayer = new MySqlCommand(queryInsertPlayer, connection))
            {
                cmdInsertPlayer.ExecuteNonQuery();
            }
            List<string> names = new List<string>();
            names.Add("Баюмка");
            names.Add("Tarahtienko");
            names.Add("DenSizzz");
            names.Add("Катя");
            names.Add("AnnaTolivna");
            names.Add("jwbaelis");
            names.Add("vilegecko");
            names.Add("knyaz_170");
            names.Add("Dari4ka7");
            names.Add("baiumka");
            names.Add("mykhailus");
            names.Add("Poiut_91");
            names.Add("black_valkyrie28");
            names.Add("Пустой слот");

            foreach (string name in names)
            {
                queryInsertPlayer = "INSERT INTO people (id_user, reg_date, name) VALUES (@id_user, @reg_date, @name);";
                using (MySqlCommand cmdInsertPlayer = new MySqlCommand(queryInsertPlayer, connection))
                {
                    cmdInsertPlayer.Parameters.AddWithValue("@id_user", 1);
                    cmdInsertPlayer.Parameters.AddWithValue("@reg_date", DateTime.Now.Date);
                    cmdInsertPlayer.Parameters.AddWithValue("@name", name);
                    cmdInsertPlayer.ExecuteNonQuery();
                }
            }*/

            return connection;
        }
        catch (Exception ex)
        {
            string text = "";
            text += $"Connect Error: {ex.Message}, {connection.Database}\n";
            if (ex.InnerException != null)
            {
                text += $"Inner Error: {ex.InnerException.Message}";
            }
            OnDataBaseError.Invoke(text);
            return null;
        }
    }

    private void SuccssesLogin(int id, int userNumber, string name)
    {        
        OnUserLogin?.Invoke(userNumber, name);
        userID = id;
        LoadPeopleList();
        isLogin = true;
    }

    public void Register(string login, string password, string name)
    {
        try
        {
            // Пользователь не найден, создаем нового
            string queryGetMaxNumber = "SELECT MAX(number) FROM users";
            int newNumber = 1;

            using (MySqlCommand cmdGetMaxNumber = new MySqlCommand(queryGetMaxNumber, Conn))
            {
                object result = cmdGetMaxNumber.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    newNumber = Convert.ToInt32(result) + 1;
                }
            }

            // Добавляем нового пользователя
            string queryInsertUser = "INSERT INTO users (number, login, password, name) VALUES (@number, @login, @password, @name); SELECT LAST_INSERT_ID();";
            using (MySqlCommand cmdInsertUser = new MySqlCommand(queryInsertUser, Conn))
            {
                cmdInsertUser.Parameters.AddWithValue("@number", newNumber);
                cmdInsertUser.Parameters.AddWithValue("@login", login);
                cmdInsertUser.Parameters.AddWithValue("@password", password);
                cmdInsertUser.Parameters.AddWithValue("@name", name);

                int newId = Convert.ToInt32(cmdInsertUser.ExecuteScalar());
                //SuccssesLogin(newId, newNumber);
            }
        }
        catch (Exception ex)
        {
            string text = "";
            text += $"Register Error: {ex.Message}, {connection.Database}\n";
            if (ex.InnerException != null)
            {
                text += $"Inner Error: {ex.InnerException.Message}";
            }
            OnDataBaseError.Invoke(text);
        }
    }

    public void Login(string login, string password)
    {
        try
        {
            // Проверяем, существует ли пользователь с таким логином
            string queryCheckUser = "SELECT number, password,id,name FROM users WHERE login = @login";
            using (MySqlCommand cmdCheckUser = new MySqlCommand(queryCheckUser, Conn))
            {
                cmdCheckUser.Parameters.AddWithValue("@login", login);
                int userNumber = 0;
                int id = 0;
                string name = "";
                using (MySqlDataReader reader = cmdCheckUser.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Пользователь найден, проверяем пароль
                        string storedPassword = reader.GetString("password");
                        if (storedPassword == password)
                        {
                            userNumber = reader.GetInt32("number");
                            id = reader.GetInt32("id");
                            name = reader.GetString("name");
                        }
                        else
                        {
                            OnDataBaseError?.Invoke(Translator.Message(Messages.WRONG_PASSWORD)); ;
                        }
                    }
                }
                if (userNumber != 0 && id != 0)
                {
                    SuccssesLogin(id, userNumber, name);
                    return;
                }
            }


        }
        catch (Exception ex)
        {
              string text = "";
              text += $"Register Error: {ex.Message}, {connection.Database}\n";
              if (ex.InnerException != null)
              {
                  text += $"Inner Error: {ex.InnerException.Message}";
              }
              OnDataBaseError.Invoke(text);
          
        }
    }



        public static People GetRandomPlayer()
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

    public void AddPeople(string name)
    {
        if (isLogin)
        {
            try
            {
                string queryInsertPeople = "INSERT INTO people (name, id_user) VALUES (@name, @id_user); SELECT LAST_INSERT_ID();";
                using (MySqlCommand cmdInsertPeople = new MySqlCommand(queryInsertPeople, Conn))
                {
                    cmdInsertPeople.Parameters.AddWithValue("@name", name);
                    cmdInsertPeople.Parameters.AddWithValue("@id_user", userID);
                    int newId = Convert.ToInt32(cmdInsertPeople.ExecuteScalar());
                    People newPeople = new People(newId, name);
                    peopleList.Add(newPeople);
                    OnPeopleAdded?.Invoke(newPeople);
                }
            }
            catch (Exception ex)
            {
                OnDataBaseError?.Invoke($"Connect Error: {ex.Message}, {connection.Database}");
                if (ex.InnerException != null)
                {
                    OnDataBaseError?.Invoke($"Inner Error: {ex.InnerException.Message}");
                }                
            }
        }
    }

    public void WriteResult(Game game)
    {
        if (isLogin)
        {
            try
            {
                string queryInsertGame = "INSERT INTO games (dd, dt, id_user) VALUES (@dd, @dt, @id_user); SELECT LAST_INSERT_ID();";
                using (MySqlCommand cmdInsertGame = new MySqlCommand(queryInsertGame, Conn))
                {
                    cmdInsertGame.Parameters.AddWithValue("@dd", DateTime.Now.Date);
                    cmdInsertGame.Parameters.AddWithValue("@dt", DateTime.Now.ToString());
                    cmdInsertGame.Parameters.AddWithValue("@id_user", userID                        );
                    int newGameId = Convert.ToInt32(cmdInsertGame.ExecuteScalar());
                    foreach(Player p in game.Players)
                    {
                        string queryInsertPlayer = "INSERT INTO players (id_game, id_people, role, id_place, is_alive) VALUES (@id_game, @id_people, @role, @id_place, @is_alive);";
                        using (MySqlCommand cmdInsertPlayer = new MySqlCommand(queryInsertPlayer, Conn))
                        {
                            cmdInsertPlayer.Parameters.AddWithValue("@id_game", newGameId);
                            cmdInsertPlayer.Parameters.AddWithValue("@id_people", p.People.id);
                            cmdInsertPlayer.Parameters.AddWithValue("@role", p.Role.ToString());
                            cmdInsertPlayer.Parameters.AddWithValue("@id_place", p.Number);
                            cmdInsertPlayer.Parameters.AddWithValue("@is_alive", !p.IsDead);
                            cmdInsertPlayer.ExecuteNonQuery();                   
                        }
                    }
                    foreach(History.HistoryEvent eve in game.history.Events)
                    {
                        string queryInsertHistory = "INSERT INTO history (game, player, target, event_type, dd, dt) VALUES (@game, @player, @target, @event_type, @dd, @dt);";
                        using (MySqlCommand cmdInsertHistory = new MySqlCommand(queryInsertHistory, Conn))
                        {
                            cmdInsertHistory.Parameters.AddWithValue("@game", newGameId);

                            if(eve.player == null ) cmdInsertHistory.Parameters.AddWithValue("@player", 0);
                            else cmdInsertHistory.Parameters.AddWithValue("@player", eve.player.Number);

                            if (eve.target == null) cmdInsertHistory.Parameters.AddWithValue("@target", 0);
                            else cmdInsertHistory.Parameters.AddWithValue("@target", eve.target.Number);

                            cmdInsertHistory.Parameters.AddWithValue("@event_type", eve.type.ToString());
                            cmdInsertHistory.Parameters.AddWithValue("@dd", eve.dateTime.Date);
                            cmdInsertHistory.Parameters.AddWithValue("@dt", eve.dateTime.ToString());
                            cmdInsertHistory.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnDataBaseError?.Invoke($"Connect Error: {ex.Message}, {connection.Database}");
                if (ex.InnerException != null)
                {
                    OnDataBaseError?.Invoke($"Inner Error: {ex.InnerException.Message}");
                }
            }
        }
    }

    public void StartNewGame(Game game)
    {
        throw new NotImplementedException();
    }

    public void WriteHistory(History.HistoryEvent historyEvent)
    {
        throw new NotImplementedException();
    }

    public void EndGame(Game game)
    {
        throw new NotImplementedException();
    }

    public void EndGame(Game game, int winner)
    {
        throw new NotImplementedException();
    }

    public void RefreshGameList()
    {
        throw new NotImplementedException();
    }

    public void GetGameDetails()
    {
        throw new NotImplementedException();
    }

    public void GetGameDetails(GameInfo game)
    {
        throw new NotImplementedException();
    }
}
