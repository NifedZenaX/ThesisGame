using System;
using System.Collections.Generic;

public class ForgotPasswordModule : BaseModule
{
    double hourDegree, minuteDegree;
    TimeSpan time;

    public override bool? CheckAnswer()
    {
        throw new NotImplementedException();
    }

    public override void GenerateProblem()
    {
        Random rand = new Random();
        TimeSpan start = TimeSpan.FromHours(0);
        TimeSpan end = TimeSpan.FromHours(23);
        int maxMins = (int)(end - start).TotalMinutes;
        int mins = rand.Next(maxMins);
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
        throw new NotImplementedException();
    }

    public override void ResetAnswer()
    {
        throw new NotImplementedException();
    }

    public override void SubmitAnswer(object answer)
    {
        throw new NotImplementedException();
    }
}
