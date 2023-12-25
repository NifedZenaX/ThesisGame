using System;
using System.Collections.Generic;

public class ForgotPasswordModule : BaseModule
{
    public override bool CheckAnswer(string answer)
    {
        throw new NotImplementedException();
    }

    public override void GenerateProblem()
    {
        Random rand = new Random();
        TimeSpan start = TimeSpan.FromHours(0);
        TimeSpan end = TimeSpan.FromHours(11);
        int maxMins = (int) (end - start).TotalMinutes;
        int mins = rand.Next(maxMins);
        TimeSpan time = start.Add(TimeSpan.FromMinutes(mins));

        problem = time.ToString();
    }
}
