using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PeopleObject : MonoBehaviour
{
    [SerializeField] TMP_Text nicknameText;
    private People peopleInfo;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(onClick);
    }

    private void onClick()
    {
        Controller.singlton.PeopleClick(peopleInfo);
    }

    public void SetPeople(People people)
    {
        this.peopleInfo = people;
        RedrawPeople();
    }

    private void RedrawPeople()
    {
        nicknameText.text = peopleInfo.Nickname;
    }
}
