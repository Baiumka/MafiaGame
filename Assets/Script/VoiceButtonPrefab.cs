using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class VoiceButtonPrefab : MonoBehaviour
{
    private int voices;
    internal void Init(int i)
    {
        transform.GetChild(0).GetComponent<TMP_Text>().text = i.ToString();
        GetComponent<Button>().onClick.AddListener(onClick);
        voices = i;
    }

    private void onClick()
    {
        Controller.singlton.SetVoices(voices);
    }
}
