using UnityEngine;
using TMPro;
public class GameListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private TMP_Text winerText;

    private GameInfo gameInfo;

    public void Init(GameInfo gameInfo)
    {
        this.gameInfo = gameInfo;

        dateText.text = gameInfo.dateTime.Date.ToString();
    }
}
