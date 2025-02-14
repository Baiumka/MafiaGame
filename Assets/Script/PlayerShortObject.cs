using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerShortObject : MonoBehaviour
{
    [SerializeField] private Image numberPlate;
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private TMP_Text nicknameText;


    public void Init(ResultPlayer player)
    {
        switch (player.role) 
        {
            case Role.CITIZEN:
                numberPlate.color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
                numberText.color = ColorStore.store.CITIZEN_TEXT_COLOR;
                break;
            case Role.SHERIFF:
                numberPlate.color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
                numberText.color = ColorStore.store.CITIZEN_TEXT_COLOR;
                break;
            case Role.MAFIA:
                numberPlate.color = ColorStore.store.MAFIA_BACKGROUND_COLOR;
                numberText.color = ColorStore.store.MAFIA_TEXT_COLOR;
                break;
            case Role.BOSS:
                numberPlate.color = ColorStore.store.MAFIA_BACKGROUND_COLOR;
                numberText.color = ColorStore.store.MAFIA_TEXT_COLOR;
                break;
        }
        nicknameText.text = player.nickname;
        if (player.is_alive == false)
        {
            nicknameText.color = Color.black;
            nicknameText.fontStyle = FontStyles.Strikethrough;
        }
        else
        {
            nicknameText.color = Color.white;
        }
        numberText.text = player.number.ToString();

        
    }

    
}
