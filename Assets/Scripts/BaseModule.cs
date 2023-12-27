using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseModule
{
    public object problem { get; protected set; }
    public object solution { get; protected set; }

    public abstract void GenerateProblemAndSolution();

    public bool CheckAnswer(object answer) {
        return answer.Equals(solution);
    }
}
