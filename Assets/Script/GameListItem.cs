using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class GameListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private TMP_Text winerText;

    private GameInfo gameInfo;

    public void Init(GameInfo gameInfo)
    {
        this.gameInfo = gameInfo;
        this.GetComponent<Button>().onClick.AddListener(onClick);
        dateText.text = gameInfo.dd.Date.ToString("dd.MM.yyyy");
        numberText.text = "Игра " + gameInfo.id;
        if (gameInfo.is_end)
        {
            switch (gameInfo.winner)
            {
                case 1:
                    winerText.text = Translator.Message(Messages.CITIZEN_WIN) + "\n" + gameInfo.citizen_alive + ":" + gameInfo.mafia_alive;
                    break;
                case 2:
                    winerText.text = Translator.Message(Messages.MAFIA_WIN) + "\n" + gameInfo.citizen_alive + ":" + gameInfo.mafia_alive;
                    break;
                default:
                    winerText.text = Translator.Message(Messages.NO_WIN);
                    break;
            }
        }
        else 
        {
            winerText.text = Translator.Message(Messages.NO_END);
        }
    }

    private void onClick()
    {
        Controller.singlton.ShowFullGameInfo(gameInfo);
    }
}
