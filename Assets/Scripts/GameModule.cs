using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModule : MonoBehaviour
{
    [SerializeField]
    private ModuleMapping.ModuleTypeEnum moduleType;
    private BaseModule module;

    private void OnEnable()
    {
        module = (BaseModule)ModuleMapping.moduleMapping[moduleType];
        module.gameModule = this;
        GetProblem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ModuleManager.Instance.SpawnGameModule();
        }

        if (module.GetType() == typeof(WiresModule))
        {
            if ((module.solution as List<WiresModule.WireNumber>) != null)
            {
                string debug = "";
                foreach (WiresModule.WireNumber wn in module.solution as List<WiresModule.WireNumber>)
                {
                    debug += $"[number: {wn.number}, answer: {wn.isAnswer}]; ";
                }
                Debug.Log(debug);
            }
        }

        bool? checkAnswer = module.CheckAnswer();
        if (checkAnswer == true && checkAnswer != null)
        {
            ModuleManager.Instance.FinishModuleMock();
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
