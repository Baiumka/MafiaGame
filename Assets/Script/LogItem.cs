using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LogItem : MonoBehaviour
{
    private ResultHistoryEvent eventInfo;
    [SerializeField] private Image playerNumberPlate;
    [SerializeField] private TMP_Text playerNumberText;
    [SerializeField] private TMP_Text playerNickname;
    [SerializeField] private GameObject playerObject;

    [SerializeField] private Image targetNumberPlate;
    [SerializeField] private TMP_Text targetNumberText;
    [SerializeField] private TMP_Text targetNickname;
    [SerializeField] private GameObject targetObject;

    [SerializeField] private TMP_Text actionText;

  

    public void SetColor(Color color)
    {
        GetComponent<Image>().color = color;
    }

    public void Init(ResultHistoryEvent e)
    {
        this.eventInfo = e;

        if(e.player_number == 0)
        {
            eventInfo.player_number = eventInfo.target_number;
            eventInfo.player_name = eventInfo.target_name;
            eventInfo.player_role = eventInfo.target_role;

            eventInfo.target_number = 0;
            eventInfo.target_name = string.Empty;
            eventInfo.target_role = Role.NONE;
        }

        DrawPlayer();
        DrawAction();
        DrawTarget();
        
        
        

        
    }

    private void DrawAction()
    {
        actionText.text = Translator.Action(eventInfo.event_type);
    }

    private void DrawTarget()
    {
        if (eventInfo.target_number == 0)
        {
            targetNumberText.text = string.Empty;
            targetNickname.text = string.Empty;
        }
        else
        {
            targetNumberText.text = eventInfo.target_number.ToString();
            targetNickname.text = eventInfo.target_name.ToString();
            if (eventInfo.target_role == Role.MAFIA || eventInfo.target_role == Role.BOSS)
            {
                targetNumberPlate.color = ColorStore.store.MAFIA_BACKGROUND_COLOR;
                targetNumberText.color = ColorStore.store.MAFIA_TEXT_COLOR;
            }
            else
            {
                targetNumberPlate.color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
                targetNumberText.color = ColorStore.store.CITIZEN_TEXT_COLOR;
            }
        }

    }

    private void DrawPlayer()
    {
        if (eventInfo.player_number == 0)
        {
            playerNumberText.text = string.Empty;
            playerNickname.text = string.Empty;
        }
        else
        {
            playerNumberText.text = eventInfo.player_number.ToString();
            playerNickname.text = eventInfo.player_name.ToString();
            if (eventInfo.player_role == Role.MAFIA || eventInfo.player_role == Role.BOSS)
            {
                playerNumberPlate.color = ColorStore.store.MAFIA_BACKGROUND_COLOR;
                playerNumberText.color = ColorStore.store.MAFIA_TEXT_COLOR;
            }
            else
            {
                playerNumberPlate.color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
                playerNumberText.color = ColorStore.store.CITIZEN_TEXT_COLOR;
            }
        }
    }


}
