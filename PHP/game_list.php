<?php
// Подключение к базе данных через внешний файл
require_once 'db.php'; // Подключение уже установлено в этом файле

// Установка заголовка для JSON-ответа
header('Content-Type: application/json');

if ($_SERVER['REQUEST_METHOD'] === 'GET') {
    // Проверка наличия параметра id_user
    if (isset($_GET['id_user']) && is_numeric($_GET['id_user'])) {
        $idUser = (int)$_GET['id_user'];

        try {
            // Подготовка и выполнение запроса к функции
            $query = "SELECT * FROM mafia.p_get_games_by_user(:id_user)";
            $stmt = $pdo->prepare($query);
            $stmt->bindParam(':id_user', $idUser, PDO::PARAM_INT);
            $stmt->execute();

            // Получение результатов
            $games = $stmt->fetchAll(PDO::FETCH_ASSOC);

            // Возврат данных в формате JSON
            echo json_encode([
                "status" => "success",
                "games" => $games
            ], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
        } catch (Exception $e) {
            // Обработка ошибок
            echo json_encode([
                "status" => "error",
                "message" => "Ошибка выполнения запроса: " . $e->getMessage()
            ], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
        }
    } else {
        // Ошибка, если параметр id_user отсутствует или некорректный
        echo json_encode([
            "status" => "error",
            "message" => "Параметр 'id_user' обязателен и должен быть числом."
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
