using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseModule
{
    public string problem { get; protected set; }

    public abstract void GenerateProblem();

    public bool CheckAnswer(string answer) {
        if (answer.Equals(problem))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
