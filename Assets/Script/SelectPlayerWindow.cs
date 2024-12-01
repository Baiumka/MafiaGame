using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayerWindow : MonoBehaviour
{
    [SerializeField] GameObject peopleObjectPrefab;
    [SerializeField] Transform spawnPanel;


    private List<PeopleObject> peopleObjectList;

    public void DrawPeople()
    {
        peopleObjectList = new List<PeopleObject>();   
        foreach(Transform child in spawnPanel)
        {
            Destroy(child.gameObject);
        }
        foreach (People people in Controller.AvaiblePeople)
        {
            GameObject newPeopleObject = GameObject.Instantiate(peopleObjectPrefab, spawnPanel);
            PeopleObject po = newPeopleObject.GetComponent<PeopleObject>();
            
            po.SetPeople(people);
            peopleObjectList.Add(po);
        }
        
    }    
}
