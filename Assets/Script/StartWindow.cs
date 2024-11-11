using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartWindow : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] TMP_InputField playerCountField;
    [SerializeField] Button playersButton;
    
    void Start()
    {
        startButton.onClick.AddListener(onStartClick);   
        playersButton.onClick.AddListener(onPlayerClick);   
    }

    private void onPlayerClick()
    {
        Controller.singlton.ShowPlayerList();
    }

    private void onStartClick()
    {
        Controller.singlton.CreateGame(10);
    }

}
