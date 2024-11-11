using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RolePrefab : MonoBehaviour
{
    public Role role;
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text backgroundText;
    [SerializeField] private TMP_Text nicknameText;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);

        switch(role)
        {
            case Role.CITIZEN:
                background.color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
                backgroundText.color = ColorStore.store.CITIZEN_TEXT_COLOR;
                backgroundText.text = "C";
                break;
            case Role.SHERIFF:
                background.color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
                backgroundText.color = ColorStore.store.CITIZEN_TEXT_COLOR;
                backgroundText.text = "S";
                break;
            case Role.MAFIA:
                background.color = ColorStore.store.MAFIA_BACKGROUND_COLOR;
                backgroundText.color = ColorStore.store.MAFIA_TEXT_COLOR;
                backgroundText.text = "M";
                break;
            case Role.BOSS:
                background.color = ColorStore.store.MAFIA_BACKGROUND_COLOR;
                backgroundText.color = ColorStore.store.MAFIA_TEXT_COLOR;
                backgroundText.text = "B";
                break;
            default:
                background.color = ColorStore.store.NONE_BACKGROUND_COLOR;
                backgroundText.color = ColorStore.store.NONE_TEXT_COLOR;
                backgroundText.text = "?";
                break;
        }
        nicknameText.text = role.ToString();
    }

    private void OnClick()
    {
        Controller.singlton.SelectRole(role);
    }
}
