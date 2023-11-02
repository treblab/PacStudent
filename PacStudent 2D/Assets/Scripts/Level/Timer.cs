using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Class created for management of the in-game timer.

public class Timer : MonoBehaviour
{
    public Text timerText;
    private TimeSpan timePlaying;
    private bool timerGoing;

    void Start()
    {
        // Initialize timer variables
        timerGoing = false;
        timePlaying = TimeSpan.Zero;
        timerText.text = "00:00:00";
    }

    public void StartTimer()
    {
        timerGoing = true;
        timePlaying = TimeSpan.Zero;
        StartCoroutine(UpdateTimer());
    }

    public void StopTimer()
    {
        timerGoing = false;
    }

    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            timePlaying = timePlaying.Add(TimeSpan.FromSeconds(1));
            timerText.text = timePlaying.ToString("hh':'mm':'ss");
            yield return new WaitForSeconds(1);
        }
    }
}
