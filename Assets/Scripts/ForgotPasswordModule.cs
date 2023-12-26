using System;

public class ForgotPasswordModule : BaseModule
{
    public override bool CheckAnswer(string answer)
    {
        throw new NotImplementedException();
    }

    public override void GenerateProblemAndSolution()
    {
        #region Generate Problem
        Random rand = new Random();
        TimeSpan start = TimeSpan.FromHours(0);
        TimeSpan end = TimeSpan.FromHours(23);
        int maxMins = (int) (end - start).TotalMinutes;
        int mins = rand.Next(maxMins);
        TimeSpan time = start.Add(TimeSpan.FromMinutes(mins));

        problem = time.ToString();
        #endregion

        #region Generate Solution
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

        double hourDegree = time.Hours % 12 * 30 + time.Minutes * 0.5;
        double minuteDegree = time.Minutes * 6;

        if((hourDegree - minuteDegree) >= 0)
        {
            solution = onesMinute.ToString() + tensMinute.ToString() + onesHour.ToString() + tensHour.ToString();
        }
        else
        {
            if(hourDegree < 180)
            {
                solution = tensMinute.ToString() + onesMinute.ToString() + tensHour.ToString() + onesHour.ToString();
            }
            else
            {
                solution = tensHour.ToString() + onesHour.ToString() + tensMinute.ToString() + onesMinute.ToString();
            }
        }
        #endregion
    }
}
