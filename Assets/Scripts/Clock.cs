using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Clock : MonoBehaviour
{
    public float startTime;
    float gameTimer;

    public TextMeshProUGUI timeTxt;
    public Image TimeFill;

    bool gameStarted;

    public delegate void ClockEvent();
    public static ClockEvent StartEvent;
    public static ClockEvent OverEvent;

    private void OnEnable()
    {
        StartEvent += StartTimer;
    }

    private void OnDisable()
    {
        StartEvent -= StartTimer;
    }

    public void StartTimer()
    {
        gameTimer = startTime;
        gameStarted = true;
    }

    private void Update()
    {
        if (gameStarted)
        {
            gameTimer -= Time.deltaTime;

            if (gameTimer <= 0)
            {
                gameStarted = false;
                OverEvent?.Invoke();
                //start game over
            }
        }

        UpdateClockFace();
    }

    private void UpdateClockFace()
    {
        timeTxt.text = string.Format("{0}", (int)gameTimer);

        var fillValue = gameTimer / startTime;
        TimeFill.fillAmount = fillValue;

        //var clockArmValue = ExtensionMethod.Remap(fillValue, 0, 1, 0, 360);
    }
}
