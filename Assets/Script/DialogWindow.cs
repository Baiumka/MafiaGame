using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogWindow : MonoBehaviour
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private TMP_Text headText;

    
    public bool IsShown { get => gameObject.activeSelf; }

    public void ShowDialog(string message, System.Action onYes, System.Action onNo)
    {
        gameObject.SetActive(true);
        headText.text = message;

        // Привязываем действия к кнопкам
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() =>
        {
            onYes?.Invoke();
            CloseDialog();
        });

        noButton.onClick.AddListener(() =>
        {
            onNo?.Invoke();
            CloseDialog();
        });
    }

    // Скрыть диалоговое окно
    public void CloseDialog()
    {
        gameObject.SetActive(false);
    }

}
