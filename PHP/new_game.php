<?php
// Подключение к базе данных через внешний файл
require_once 'db.php'; // Подключение уже установлено в этом файле

// Установка заголовка для JSON-ответа
header('Content-Type: application/json');

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    // Получение JSON-данных из тела запроса
    $input = file_get_contents('php://input');
    $data = json_decode($input, true);
	$players = $data['players'];
	

    if (isset($players) && is_array($players)) {
     
        try {
            // Начало транзакции
            $pdo->beginTransaction();

            // Создание новой игры
            $queryGame = "INSERT INTO mafia.games (dd, id_user) VALUES (NOW(), :id_user) RETURNING id;";
            $stmtGame = $pdo->prepare($queryGame);
			$stmtGame->bindParam(':id_user', $data['id_user'], PDO::PARAM_INT);
            $stmtGame->execute();
            $gameId = $stmtGame->fetchColumn();

            // Добавление игроков
            $queryPlayer = "INSERT INTO mafia.players (id_game, number, id_people, role) VALUES (:game_id, :number, :id_people, :role)";
            $stmtPlayer = $pdo->prepare($queryPlayer);

            foreach ($players as $player) {
                $stmtPlayer->bindParam(':game_id', $gameId, PDO::PARAM_INT);
                $stmtPlayer->bindParam(':number', $player['Number'], PDO::PARAM_INT);
                $stmtPlayer->bindParam(':id_people', $player['People']['id'], PDO::PARAM_INT);
                $stmtPlayer->bindParam(':role', $player['Role'], PDO::PARAM_INT);
                $stmtPlayer->execute();
            }

            // Завершение транзакции
            $pdo->commit();

            // Возврат ID созданной игры
            echo json_encode(["status" => "success", "id" => $gameId], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
        } catch (Exception $e) {
            // Откат транзакции в случае ошибки
            $pdo->rollBack();
            echo json_encode(["status" => "error", "message" => "Ошибка создания игры: " . $e->getMessage()], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
        }
    } else {
        echo json_encode(["status" => "error", "message" => "Параметры 'player_number' и 'player_id_people' обязательны и должны быть массивами."], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
    }
} else {
    echo json_encode(["status" => "error", "message" => "Метод запроса должен быть POST."], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
}
?>
