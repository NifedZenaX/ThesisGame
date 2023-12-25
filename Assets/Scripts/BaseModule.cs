using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseModule
{
    public string problem { get; protected set; }
    public string solution { get; protected set; }

    public abstract void GenerateProblemAndSolution();

    public abstract bool CheckAnswer(string answer);
}
