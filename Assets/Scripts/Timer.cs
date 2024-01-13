using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private int minute;
    [SerializeField] private float second;

    [SerializeField] private TextMeshProUGUI timerText;

    private string minuteText;
    private string secondText;

    // Update is called once per frame
    void Update()
    {
        if(minute > 0 || second > 0)
        {
            if(second <= 0)
            {
                minute -= 1;
                second = 60;
            }
            second -= Time.deltaTime;
            minuteText = (minute < 10) ? "0" + minute : minute.ToString();
            secondText = (second < 10) ? "0" + (int)second : ((int)second).ToString();
            timerText.text = minuteText + ":" + secondText;
        }
        else
        {
            //Time's up
            Time.timeScale = 0;
        }
    }
}
