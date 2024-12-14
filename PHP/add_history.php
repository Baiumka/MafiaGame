<?php
// Подключение к базе данных через внешний файл
require_once 'db.php'; // Подключение уже установлено в этом файле

// Установка заголовка для JSON-ответа
header('Content-Type: application/json');

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    // Получение JSON-данных из тела запроса
    $input = file_get_contents('php://input');
    $data = json_decode($input, true);
	try {
		// Начало транзакции
		$pdo->beginTransaction();
		
		$queryGame = "INSERT INTO mafia.history (id_game, id_player, id_target, event_type, dd) VALUES ( :id_game, :id_player, :id_target, :event_type, NOW()) RETURNING id;";
		$stmtGame = $pdo->prepare($queryGame);
		$stmtGame->bindParam(':id_game', $data['id_game'], PDO::PARAM_INT);
		$stmtGame->bindParam(':id_player', $data['history']['player']['Number'], PDO::PARAM_INT);
		$stmtGame->bindParam(':id_target', $data['history']['target']['Number'], PDO::PARAM_INT);
		$stmtGame->bindParam(':event_type', $data['history']['type'], PDO::PARAM_STR);
		$stmtGame->execute();
		$historyID = $stmtGame->fetchColumn();


		// Завершение транзакции
		$pdo->commit();

		// Возврат ID созданной игры
		echo json_encode(["status" => "success", "id" => $historyID], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
	} catch (Exception $e) {
		// Откат транзакции в случае ошибки
		$pdo->rollBack();
		echo json_encode(["status" => "error", "message" => "Ошибка создания игры: " . $e->getMessage()], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
	}    
} else {
    echo json_encode(["status" => "error", "message" => "Метод запроса должен быть POST."], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
}
?>
