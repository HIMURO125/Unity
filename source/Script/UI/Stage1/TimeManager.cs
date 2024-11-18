using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms;
using System;

public class TimeManager : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text timeText;
    [SerializeField] GameObject GameOver;
    private int TimeMinutes = 3;
    private float TimeSeconds;
    private PauseManager pauseManager;
    private float CountTime; 
    public float GetTime
    {
        get { return CountTime; }
    }

    void Start()
    {
        //ï™ÇïbÇ…íºÇ∑
        TimeSeconds = TimeMinutes * 60;
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isPaused = pauseManager.IsPaused;
        if (!isPaused)
        {
            //0ïbÇ…Ç»ÇÈÇ‹Ç≈
            if (TimeSeconds >= 0)
            {
                TimeSeconds -= Time.deltaTime;
                CountTime += Time.deltaTime;
                var span = new TimeSpan(0, 0, (int)TimeSeconds);
                //30ïbà»â∫Ç≈ê‘êFÇ…
                if (TimeSeconds <= 30)
                {
                    timeText.color = Color.red;
                }
                //ï™ï™ÅFïbïbÇ≈ï\é¶
                timeText.SetText(span.ToString(@"mm\:ss"));
            }
            else
            {
                pauseManager.Pause();
                GameOver.SetActive(true);
            }
        }
    }
    public void Bonus()
    {
        TimeSeconds += 60;
    }
}
