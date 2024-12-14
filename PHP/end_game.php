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
            $queryGame = "update mafia.games g set end_date = now(), winner = :winner, mafia_alive = :mafia, citizen_alive = :citizen WHERE g.id = :id RETURNING id;";
            $stmtGame = $pdo->prepare($queryGame);
			$stmtGame->bindParam(':id', $data['id_game'], PDO::PARAM_INT);
			$stmtGame->bindParam(':winner', $data['game_winner'], PDO::PARAM_INT);
			$stmtGame->bindParam(':mafia', $data['mafia'], PDO::PARAM_INT);
			$stmtGame->bindParam(':citizen', $data['citizen'], PDO::PARAM_INT);
            $stmtGame->execute();
            $gameId = $stmtGame->fetchColumn();

            // Добавление игроков
            $queryPlayer = "update mafia.players p set is_alive = not :alive where p.id_game = :id_game and p.number = :number;";			
            

            foreach ($players as $player) {
				$stmtPlayer = $pdo->prepare($queryPlayer);
                $stmtPlayer->bindParam(':id_game', $data['id_game'], PDO::PARAM_INT);
                $stmtPlayer->bindParam(':number', $player['Number'], PDO::PARAM_INT);
                $stmtPlayer->bindParam(':alive', $player['IsDead'], PDO::PARAM_BOOL);

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
