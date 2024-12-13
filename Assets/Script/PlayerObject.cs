
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
    [SerializeField] Button voteButton;    
    [SerializeField] Button kickButton;    
    [SerializeField] Button warnButton;    
    public RoleContextHandler onRoleClicked;
    private Player playerInfo;
    private bool isHide;


    private void Start()
    {
        nicknameButton.onClick.AddListener(onNickNameClicked);
        roleButton.onClick.AddListener(onRoleButtonClicked);
        voteButton.onClick.AddListener(onVoteButtonClicked);
        kickButton.onClick.AddListener(KickButtonClicked);
        warnButton.onClick.AddListener(WarnButtonClicked);
        

        voteButton.gameObject.SetActive(false);
    }

    private void WarnButtonClicked()
    {
        Controller.singlton.WarnPlayer(playerInfo);
    }

    private void KickButtonClicked()
    {
        Controller.singlton.KickPlayer(playerInfo);
    }

    private void onVoteButtonClicked()
    {
        Controller.singlton.PutToVote(playerInfo);
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
        this.playerInfo.onPlayerPut += PutPlayer;
        this.playerInfo.onPlayerVote += VotePlayer;        
        this.playerInfo.onPlayerDie += Die;        
        this.playerInfo.onPreparedForDie += PrepareDie;        
        this.playerInfo.onAddedToBestTurn += VotePlayer;        
        this.playerInfo.onTakeWarn += WarnUpdate;        
        RedrawPlayer();
    }

    private void WarnUpdate()
    {
        warnButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "!\n" + playerInfo.Warn;
        if(playerInfo.Warn >= 3)
        {
            warnButton.GetComponent<Image>().color = Color.red;
        }
    }

    private void PrepareDie()
    {
        nicknameButton.GetComponent<Image>().color = Color.red;
    }

    private void Die()
    {
        this.gameObject.SetActive(false);
    }

    private void VotePlayer()
    {
        nicknameButton.GetComponent<Image>().color = Color.yellow;
    }

    private void UnVotePlayer()
    {
        nicknameButton.GetComponent<Image>().color = Color.white;
    }


    private void PutPlayer()
    {
        voteButton.GetComponent<Image>().color = Color.red;
    }

    private void UnPutPlayer()
    {
        voteButton.GetComponent<Image>().color = Color.white;
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
                nicknameText.text = playerInfo.People.name;
            }
            if (playerInfo.IsPutted) PutPlayer();
            else UnPutPlayer();
            if (playerInfo.Voted != null) VotePlayer();
            else UnVotePlayer();

            RedrawRole();
        }
    }

    public void HideRole()
    {
        isHide = true;
        RedrawRole();
    }
    public void ShowRole()
    {
        isHide = false;
        RedrawRole();
    }

    private void RedrawRole()
    {
        if (playerInfo != null)
        {
            if (!isHide)
            {
                TMP_Text text = roleButton.transform.GetChild(0).GetComponent<TMP_Text>();

                switch (playerInfo.Role)
                {
                    case Role.CITIZEN:
                        numberPanel.color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
                        numberText.color = ColorStore.store.CITIZEN_TEXT_COLOR;
                        roleButton.GetComponent<Image>().color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
                        text.color = ColorStore.store.CITIZEN_TEXT_COLOR;
                        text.text = "C";
                        break;
                    case Role.SHERIFF:
                        numberPanel.color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
                        numberText.color = ColorStore.store.CITIZEN_TEXT_COLOR;
                        roleButton.GetComponent<Image>().color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
                        text.color = ColorStore.store.CITIZEN_TEXT_COLOR;
                        text.text = "S";
                        break;
                    case Role.MAFIA:
                        numberPanel.color = ColorStore.store.MAFIA_BACKGROUND_COLOR;
                        numberText.color = ColorStore.store.MAFIA_TEXT_COLOR;
                        roleButton.GetComponent<Image>().color = ColorStore.store.MAFIA_BACKGROUND_COLOR;
                        text.color = ColorStore.store.MAFIA_TEXT_COLOR;
                        text.text = "M";
                        break;
                    case Role.BOSS:
                        numberPanel.color = ColorStore.store.MAFIA_BACKGROUND_COLOR;
                        numberText.color = ColorStore.store.MAFIA_TEXT_COLOR;
                        roleButton.GetComponent<Image>().color = ColorStore.store.MAFIA_BACKGROUND_COLOR;
                        text.color = ColorStore.store.MAFIA_TEXT_COLOR;
                        text.text = "B";
                        break;
                    default:
                        numberPanel.color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
                        numberText.color = ColorStore.store.CITIZEN_TEXT_COLOR;
                        roleButton.GetComponent<Image>().color = ColorStore.store.NONE_BACKGROUND_COLOR;
                        text.color = ColorStore.store.NONE_TEXT_COLOR;
                        text.text = "?";
                        break;
                }
            }
            else
            {
                TMP_Text text = roleButton.transform.GetChild(0).GetComponent<TMP_Text>();
                numberPanel.color = ColorStore.store.CITIZEN_BACKGROUND_COLOR;
                numberText.color = ColorStore.store.CITIZEN_TEXT_COLOR;
                roleButton.GetComponent<Image>().color = ColorStore.store.NONE_BACKGROUND_COLOR;
                text.color = ColorStore.store.NONE_TEXT_COLOR;
                text.text = "?";
            }        
        }
    }

    public void BlockRoleButton()
    {
        roleButton.enabled = false;
    }

    public void ShowVoteButton()
    {
        voteButton.gameObject.SetActive(true);
    }

    public void HideVoteButton()
    {
        voteButton.gameObject.SetActive(false);
    }
}
