using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseModule
{
    public object problem { get; protected set; }
    public object solution { get; protected set; }

    public void GenerateProblemAndSolution()
    {
        GenerateProblem();
        GenerateSolution();
    }

    public abstract void GenerateProblem();
    public abstract void GenerateSolution();

    public virtual bool CheckAnswer(object answer) {
        return answer.Equals(solution);
    }
}
