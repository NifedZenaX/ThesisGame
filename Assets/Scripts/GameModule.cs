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
        module.gameModule = this;
        GetProblem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            module.GenerateProblemAndSolution();
            //Debug.Log("Problem: " + module.problem.ToString() + " and Solution: " + module.solution.ToString());
        }

        if (module.CheckAnswer() == true && module.CheckAnswer() != null)
        {
            module.GenerateProblemAndSolution();
        }
    }

    //TODO: may need to change to support different types
    public object GetProblem()
    {
        module.GenerateProblemAndSolution();
        return module.problem;
    }

    public void SubmitAnswer(object answer)
    {
        ((List<object>)module.answer).Add(answer);
    }
}
