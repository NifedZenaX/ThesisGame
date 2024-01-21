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

            if (Mathf.CeilToInt(second) == 60)
            {
                minuteText = (minute < 10) ? "0" + (minute + 1) : minute.ToString();
                secondText = "00";
            }
            else
            {
                minuteText = (minute < 10) ? "0" + minute : minute.ToString();
                secondText = (Mathf.CeilToInt(second) < 10) ? "0" + Mathf.CeilToInt(second) : Mathf.CeilToInt(second).ToString();
            }
            timerText.text = minuteText + ":" + secondText;
        }
        else
        {
            //Time's up
            ScoreManager.instance.GameFinished(false);
        }
    }
}
