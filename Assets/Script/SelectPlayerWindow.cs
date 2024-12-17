using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SelectPlayerWindow : MonoBehaviour
{
    [SerializeField] GameObject peopleObjectPrefab;
    [SerializeField] Transform spawnPanel;
    [SerializeField] TMP_InputField newPlayerNameInput;
    [SerializeField] Button addNewPlayerButton;
    [SerializeField] Button exitButton;

    private List<PeopleObject> peopleObjectList;

    public void Init()
    {
        addNewPlayerButton.onClick.AddListener(AddPlayer);
        exitButton.onClick.AddListener(ExitButton);

        Controller.singlton.onPeopleAdded += DrawPeople;
    }

    private void ExitButton()
    {
        Controller.singlton.ReturnBack();
    }

    private void AddPlayer()
    {
        Controller.singlton.CreatePeople(newPlayerNameInput.text);
    }

    public void DrawList()
    {
        peopleObjectList = new List<PeopleObject>();   
        foreach(Transform child in spawnPanel)
        {
            Destroy(child.gameObject);
        }
        foreach (People people in Controller.AvaiblePeople)
        {
            DrawPeople(people);
            
        }        
    }    

    private void DrawPeople(People people)
    {
        GameObject newPeopleObject = GameObject.Instantiate(peopleObjectPrefab, spawnPanel);
        PeopleObject po = newPeopleObject.GetComponent<PeopleObject>();

        po.SetPeople(people);
        peopleObjectList.Add(po);
    }
}
