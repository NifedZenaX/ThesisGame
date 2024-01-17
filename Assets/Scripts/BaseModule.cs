using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseModule
{
    public object problem { get; protected set; }
    public object solution { get; protected set; }
    public object answer { get; set; }
    public GameModule gameModule { get; set; }

    public void GenerateProblemAndSolution()
    {
        ResetAnswer();
        GenerateProblem();
        GenerateSolution();
        LinkUIToLogic();
    }

    public abstract void GenerateProblem();
    public abstract void GenerateSolution();
    public abstract void ResetAnswer();
    public abstract bool? CheckAnswer();

    public abstract void LinkUIToLogic();
    public abstract void SubmitAnswer(object answer);
}
