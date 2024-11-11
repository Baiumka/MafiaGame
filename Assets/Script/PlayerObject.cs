
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public delegate void RoleContextHandler(Vector3 pos, Player player);
public class PlayerObject : MonoBehaviour
{
    [SerializeField] Image numberPanel;
    [SerializeField] TMP_Text numberText;
    [SerializeField] TMP_Text nicknameText;
    [SerializeField] Button nicknameButton;
    [SerializeField] Button roleButton;    
    public RoleContextHandler onRoleClicked;
    private Player playerInfo;


    private void Start()
    {
        nicknameButton.onClick.AddListener(onNickNameClicked);
        roleButton.onClick.AddListener(onRoleButtonClicked);
    }

    private void onRoleButtonClicked()
    {
        onRoleClicked?.Invoke(roleButton.transform.position, playerInfo);
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
            RedrawRole();
        }
    }

    private void RedrawRole()
    {
        if (playerInfo != null)
        {
            TMP_Text text = roleButton.transform.GetChild(0).GetComponent<TMP_Text>();

            switch (playerInfo.Role)
            {
                case Role.CITIZEN:
                    roleButton.GetComponent<Image>().color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
                    text.color = ColorStore.store.CITIZEN_TEXT_COLOR;
                    text.text = "C";
                    break;
                case Role.SHERIFF:
                    roleButton.GetComponent<Image>().color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
                    text.color = ColorStore.store.CITIZEN_TEXT_COLOR;
                    text.text = "S";
                    break;
                case Role.MAFIA:
                    roleButton.GetComponent<Image>().color = ColorStore.store.MAFIA_BACKGROUND_COLOR;
                    text.color = ColorStore.store.MAFIA_TEXT_COLOR;
                    text.text = "M";
                    break;
                case Role.BOSS:
                    roleButton.GetComponent<Image>().color = ColorStore.store.MAFIA_BACKGROUND_COLOR;
                    text.color = ColorStore.store.MAFIA_TEXT_COLOR;
                    text.text = "B";
                    break;
                default:
                    roleButton.GetComponent<Image>().color = ColorStore.store.NONE_BACKGROUND_COLOR;
                    text.color = ColorStore.store.NONE_TEXT_COLOR;
                    text.text = "?";
                    break;
            }
        }
    }



}
