using System;
using System.Collections.Generic;

public class ForgotPasswordModule : BaseModule
{
    public override void GenerateProblem()
    {
        Random rand = new Random();
        TimeSpan start = TimeSpan.FromHours(0);
        TimeSpan end = TimeSpan.FromHours(11);
        int maxMins = (int)((end - start).TotalMinutes);
        int mins = rand.Next(maxMins);
        TimeSpan time = start.Add(TimeSpan.FromMinutes(mins));

        this.problem = time.ToString();
    }
}
