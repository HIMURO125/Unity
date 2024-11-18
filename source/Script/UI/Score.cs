using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Score : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text ScoreText;
    [SerializeField] private GameObject Life;
    [SerializeField] private GameObject Timer;
    [SerializeField] private GameObject Pos;
    private TimeManager timeManager;
    private void Start()
    {
        timeManager = GameObject.FindWithTag("Timer").GetComponent<TimeManager>();
        Life.SetActive(false);
        Timer.SetActive(false);
        Pos.SetActive(false);
        ShowScore();
    }
    public void ShowScore()
    {
        float time = timeManager.GetTime;
        var span = new TimeSpan(0, 0, (int)time);
        ScoreText.SetText(span.ToString(@"mm\:ss"));
    }
}
