using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public bool stage = true;
    float timer = 0f;

    float milliseconds;
    float seconds;
    float minutes;

    public Text timeText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(stage)
        {
            timer += Time.deltaTime;
        }
        minutes = Mathf.FloorToInt(timer / 60);
        seconds = timer - minutes * 60;

        milliseconds = seconds - Mathf.FloorToInt(seconds);
        seconds = Mathf.FloorToInt(seconds);
        timeText.text = minutes + ":" + seconds + ":" + Mathf.Round(milliseconds * 100);
    }
}
