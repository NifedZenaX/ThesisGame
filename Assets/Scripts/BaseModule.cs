using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseModule<T, K>
{
    public T problem { get; protected set; }

    public K solution { get; protected set; }

    protected abstract void GenerateProblem();

    protected abstract bool CheckAnswer();
}
