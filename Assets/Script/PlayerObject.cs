
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerObject : MonoBehaviour
{
    [SerializeField] Image numberPanel;
    [SerializeField] TMP_Text numberText;
    [SerializeField] TMP_Text nicknameText;
    [SerializeField] Button nicknameButton;

    private Player playerInfo;


    private void Start()
    {
        nicknameButton.onClick.AddListener(onNickNameClicked);
    }

    private void onNickNameClicked()
    {
        Controller.singlton.SelectPeopleForSlot(playerInfo);
    }

    public void Init(Player player)
    {
        this.playerInfo = player;
        this.playerInfo.onDataUpdated += RedrawPlayer;
        RedrawPlayer();
    }

    private void RedrawPlayer()
    {
        if (playerInfo != null)
        {
            numberText.text = playerInfo.Number.ToString();
            if (playerInfo.People == null)
            {
                nicknameText.text = Translator.Message(Messages.EMPTY_SLOT);
            }
            else
            {
                nicknameText.text = playerInfo.People.Nickname;
            }
        }
    }
}
