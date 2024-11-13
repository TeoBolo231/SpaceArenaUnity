using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI textTimer;

    public static TimeSpan timePlaying;
    private bool timeGoing;

    private float elapsedTime;

    private void Start()
    {
        textTimer.text = "00:00";
        timeGoing = false;
        BeginTimer();
    }

    public void BeginTimer()
    {
        timeGoing = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer() 
    {
        timeGoing = false;
    }

    IEnumerator UpdateTimer()
    {
        while (timeGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            textTimer.text = timePlaying.ToString("mm':'ss");

            yield return null;
        }
    }

   
}
