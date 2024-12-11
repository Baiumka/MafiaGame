using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class VisualManager : MonoBehaviour
{
    private float skyRorate = 0.0f;    
    private float progress = 0.0f;
    private bool reverseCamMove;
    [SerializeField] private float skyIntensity = 1.0f;
    [SerializeField] private float skyRotateSpeed = 1.0f;
    [SerializeField] private float camSpeed = 1.0f;
    [SerializeField] private Material skyBoxDay;
    [SerializeField] private Material skyBoxNight;
    [SerializeField] private Light mainLight;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private Camera mainCam;


    public void Init()//Вместо Start()
    {
        Controller.singlton.onGameStateChanged += ChangeState;

        skyRorate = 0;
        SetDay();
    }

    private void ChangeState(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.FIRST_NIGHT_MAFIA:
                SetNight();
                break;
            case GameState.MORNING:
                SetDay();
                break;
            case GameState.NIGHT:
                SetNight();
                break;
        }
    }

    private void FixedUpdate()
    {
        progress += Time.deltaTime * camSpeed;
        progress = Mathf.Clamp01(progress); // Ограничиваем от 0 до 1
        if(reverseCamMove)
            mainCam.transform.position = Vector3.Lerp(endPos, startPos, progress);
        else
            mainCam.transform.position = Vector3.Lerp(startPos, endPos, progress);

        if (progress >= 1)
        {
            progress = 0;
            reverseCamMove = !reverseCamMove;
        }

        skyRorate += Time.deltaTime * skyRotateSpeed;
        if (skyRorate > 360) skyRorate -= 360;
        RenderSettings.skybox.SetFloat("_Rotation", skyRorate);
        RenderSettings.skybox.SetFloat("_Exposure", skyIntensity);
    }

    private void SetDay()
    {
        RenderSettings.skybox = skyBoxDay;
        skyIntensity = 1.2f;
        mainLight.intensity = 1;
    }

    private void SetNight()
    {
        RenderSettings.skybox = skyBoxNight;
        skyIntensity = 0.8f;
        mainLight.intensity = 0.11f;
    }
}
