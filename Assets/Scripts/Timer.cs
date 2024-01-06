using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private int minute;
    [SerializeField] private float second;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
            Debug.Log("Minute: " + minute + ", Second: " + (int)second);
        }
        else
        {
            //Time's up
            Time.timeScale = 0;
        }
    }
}
