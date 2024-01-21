using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class ForgotPasswordModule : BaseModule
{
    double hourDegree, minuteDegree;
    TimeSpan time;

    public override bool? CheckAnswer()
    {
        string castedAnswer = answer.ToString();
        string castedSolution = solution.ToString();

        if (castedAnswer == null)
        {
            return null;
        }

        if (castedAnswer.Length == castedSolution.Length)
        {
            if (castedSolution.Equals(castedAnswer))
            {
                return true;
            }
            else
            {
                ResetAnswer();
            }
        }

        return null;
    }

    public override void GenerateProblem()
    {
        Random rand = new Random();
        TimeSpan start = TimeSpan.FromHours(0);
        TimeSpan end = TimeSpan.FromHours(23);
        int maxMins = (int)(end - start).TotalMinutes;
        int mins = rand.Next(maxMins) / 5 * 5;
        time = start.Add(TimeSpan.FromMinutes(mins));

        hourDegree = time.Hours % 12 * 30 + time.Minutes * 0.5;
        minuteDegree = time.Minutes * 6;

        problem = new List<object>() { time.ToString(), hourDegree, minuteDegree };
    }

    public override void GenerateSolution()
    {
        int tensHour, tensMinute, onesHour, onesMinute, hour, minute;
        if (time.Minutes % 2 == 0)
        {
            hour = time.Hours % 12;
        }
        else
        {
            hour = time.Hours % 12 + 12;
        }
        minute = time.Minutes;

        tensHour = hour / 10;
        onesHour = hour % 10;

        tensMinute = minute / 10;
        onesMinute = minute % 10;

        if ((hourDegree - minuteDegree) >= 0)
        {
            solution = onesMinute.ToString() + tensMinute.ToString() + onesHour.ToString() + tensHour.ToString();
        }
        else
        {
            if (hourDegree < 180)
            {
                solution = tensMinute.ToString() + onesMinute.ToString() + tensHour.ToString() + onesHour.ToString();
            }
            else
            {
                solution = tensHour.ToString() + onesHour.ToString() + tensMinute.ToString() + onesMinute.ToString();
            }
        }
    }

    public override void LinkUIToLogic()
    {

        // problem = List<object>() { string, double, double } (time, hourDegree, minuteDegree)
        // solution = string
        List<object> castedProblem = problem as List<object>;
        string castedSolution = solution as string;

        ForgotPasswordComponents fpc = gameModule.GetComponent<ForgotPasswordComponents>();
        
        foreach (Button btn in fpc.buttons)
        {
            btn.onClick.RemoveAllListeners();
        }

        Debug.Log(castedProblem[0]);
        Debug.Log(castedSolution);

        Vector3 handRotation = fpc.hourHand.transform.eulerAngles;
        handRotation.z = (float)(double)castedProblem[1] * -1;
        fpc.hourHand.transform.eulerAngles = handRotation;

        handRotation = fpc.minuteHand.transform.eulerAngles;
        handRotation.z = (float)(double)castedProblem[2] * -1;
        fpc.minuteHand.transform.eulerAngles = handRotation;

        for (int i = 0; i < fpc.buttons.Count; i++)
        {
            int idx = i;
            if (int.TryParse(fpc.btnText[idx].text, out int btnNumber) == true)
            {
                fpc.buttons[idx].onClick.AddListener(delegate { SubmitAnswer(btnNumber); });
            }
            else
            {
                fpc.buttons[idx].onClick.AddListener(delegate { SubmitAnswer(-1); });
            }
        }
    }

    public override void ResetAnswer()
    {
        answer = "";
        gameModule.GetComponent<ForgotPasswordComponents>().inputField.text = "";
    }

    public override void SubmitAnswer(object answer)
    {
        string castedAnswer = (string)this.answer;
        int castedAddedAnswer = (int)answer;
        ForgotPasswordComponents fpc = gameModule.GetComponent<ForgotPasswordComponents>();

        // Backspace
        if (castedAddedAnswer == -1)
        {
            if (castedAnswer.Length >= 1)
            {
                castedAnswer = castedAnswer.Substring(0, castedAnswer.Length - 1);
            }
        }

        // Actual Button Number
        else
        {
            castedAnswer += castedAddedAnswer.ToString();
        }

        this.answer = castedAnswer;
        fpc.inputField.text = this.answer.ToString();
    }
}
