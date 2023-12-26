using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModule : MonoBehaviour
{
    [SerializeField]
    private ModuleMapping.ModuleTypeEnum moduleType;

    private BaseModule module;

    private void Start()
    {
        //TODO: generate random number between 0-enumCount to randomly generate module
        module = (BaseModule)ModuleMapping.moduleMapping[moduleType];
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            module.GenerateProblemAndSolution();
            Debug.Log("Problem: " + module.problem + " and Solution: " + module.solution);
        }
    }

    public string GetProblem()
    {
        module.GenerateProblemAndSolution();
        return module.problem;
    }

    public void SubmitAnswer(string answer)
    {
        bool isCorrect = module.CheckAnswer(answer);
        if (isCorrect)
        {
            //TODO: if answer is correct
            Debug.Log("You answered this question correctly!");
        }
        else
        {
            //TODO: if answer is wrong
            Debug.Log("You answered this question incorrectly!");
        }
    }
}
