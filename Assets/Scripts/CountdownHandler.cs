using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownHandler : MonoBehaviour
{
    public static CountdownHandler Singleton;
    
    public float startTime = 600f;
    public List<TMP_Text> countdownTexts;
    public bool ended = false;

    public float remainingTime;
    private bool _running = false;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    private void Update()
    {
        if (!_running) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0;
            _running = false;
            ended = true;
        }
        
        UpdateCountdownText();
    }

    public void StartCountdown()
    {
        remainingTime = startTime;
        _running = true;
        ended = false;
    }

    private void UpdateCountdownText()
    {
        float minutes = Mathf.FloorToInt(remainingTime / 60);
        float seconds = Mathf.FloorToInt(remainingTime % 60);
        
        foreach (TMP_Text text in countdownTexts)
        {
            text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
