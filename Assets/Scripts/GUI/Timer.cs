using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float CurrentTime { get; private set; }

    void Start()
    {
        
    }

    public Timer(float currentTime)
    {
        CurrentTime = currentTime;
    }

    public string ConvertToMinutes(float currentTime)
    {
        if (currentTime < 0)
            currentTime = 0;

        float minutes = Mathf.Floor(currentTime / 60);
        float sec = currentTime - minutes * 60;

        return (minutes + ":" + Mathf.Floor(sec));
    }

    public void RemoveTime(float deltaTime)
    {
        if (CurrentTime <= 0) return;

        CurrentTime -= deltaTime;
    }
}
