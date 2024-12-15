using UnityEngine;

public class ResultHistoryEvent : MonoBehaviour
{
    public string dd; // Дата и время события
    public string event_type; // Тип события
    public int player_number; // Номер игрока, инициировавшего событие
    public string player_name; // Имя игрока, инициировавшего событие
    public string player_role; // Роль игрока, инициировавшего событие
    public int target_number; // Номер игрока, ставшего целью события
    public string target_name; // Имя цели
    public string target_role; // Роль цели
}
