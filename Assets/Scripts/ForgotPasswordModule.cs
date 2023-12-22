using System;
using System.Collections.Generic;

public class ForgotPasswordModule : BaseModule<string, string>
{
    protected override bool CheckAnswer()
    {
        throw new System.NotImplementedException();
    }

    protected override void GenerateProblem()
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
