<?php
// Подключение к базе данных через внешний файл
require_once 'db.php'; // Подключение уже установлено в этом файле

// Установка заголовка для JSON-ответа
header('Content-Type: application/json');

if ($_SERVER['REQUEST_METHOD'] === 'GET') {
    // Проверка наличия параметра id_game
    if (isset($_GET['id_game']) && is_numeric($_GET['id_game'])) {
        $idGame = (int)$_GET['id_game'];

        try {
            // Подготовка и выполнение запроса к функции p_get_players_by_game
            $queryPlayers = "SELECT * FROM mafia.p_get_players_by_game(:id_game)";
            $stmtPlayers = $pdo->prepare($queryPlayers);
            $stmtPlayers->bindParam(':id_game', $idGame, PDO::PARAM_INT);
            $stmtPlayers->execute();
            $players = $stmtPlayers->fetchAll(PDO::FETCH_ASSOC);

            // Подготовка и выполнение запроса к функции p_get_log_by_game
            $queryLog = "SELECT * FROM mafia.p_get_log_by_game(:id_game)";
            $stmtLog = $pdo->prepare($queryLog);
            $stmtLog->bindParam(':id_game', $idGame, PDO::PARAM_INT);
            $stmtLog->execute();
            $log = $stmtLog->fetchAll(PDO::FETCH_ASSOC);

            // Возврат объединенного результата в формате JSON
            echo json_encode([
                "status" => "success",
                "players" => $players,
                "log" => $log
            ], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
        } catch (Exception $e) {
            // Обработка ошибок
            echo json_encode([
                "status" => "error",
                "message" => "Ошибка выполнения запроса: " . $e->getMessage()
            ], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
        }
    } else {
        // Ошибка, если параметр id_game отсутствует или некорректный
        echo json_encode([
            "status" => "error",
            "message" => "Параметр 'id_game' обязателен и должен быть числом."
        ], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
    }
} else {
    // Ошибка, если метод запроса не GET
    echo json_encode([
        "status" => "error",
        "message" => "Метод запроса должен быть GET."
    ], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
}
?>