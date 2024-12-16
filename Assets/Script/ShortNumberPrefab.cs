using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShortNumberPrefab : MonoBehaviour
{
    [SerializeField] private TMP_Text numberText;

    public void SetNumber(int number, Role role)
    {
        if(number == 0)
        {
            numberText.text = "?";
        }
        else
        {
            numberText.text = number.ToString();
        }

        switch (role)
        {
            case Role.CITIZEN:                
                numberText.color = ColorStore.store.CITIZEN_TEXT_COLOR;
                numberText.transform.parent.GetComponent<Image>().color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
                break;
            case Role.SHERIFF:
                numberText.color = ColorStore.store.CITIZEN_TEXT_COLOR;
                numberText.transform.parent.GetComponent<Image>().color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
                break;
            case Role.MAFIA:
                numberText.color = ColorStore.store.MAFIA_TEXT_COLOR;
                numberText.transform.parent.GetComponent<Image>().color = ColorStore.store.MAFIA_BACKGROUND_COLOR;
                break;
            case Role.BOSS:
                numberText.color = ColorStore.store.MAFIA_TEXT_COLOR;
                numberText.transform.parent.GetComponent<Image>().color = ColorStore.store.MAFIA_BACKGROUND_COLOR;
                break;
            default:
                numberText.color = ColorStore.store.NONE_TEXT_COLOR;
                numberText.transform.parent.GetComponent<Image>().color = ColorStore.store.NONE_BACKGROUND_COLOR;
                break;
        }
    }

}
