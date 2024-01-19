using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Random = System.Random;

public class WiresModule : BaseModule
{
    Random rand = new Random();
    int totalWires;
    List<ColorEnum> wires;
    bool hasCheckInCurrentFrame = false;
    
    public struct WireNumber
    {
        public ColorEnum color;
        public int number;
        public bool isAnswer;
    }

    private ColorEnum GetRandomColor()
    {
        Array wireEnums = Enum.GetValues(typeof(ColorEnum));
        return (ColorEnum)wireEnums.GetValue(rand.Next(wireEnums.Length));
    }

    private void SetCorrectAnswer(List<WireNumber> number, int index)
    {
        WireNumber n = number[index];
        n.isAnswer = true;
        number[index] = n;
    }

    public override void GenerateProblem()
    {
        totalWires = rand.Next(3, 6);
        wires = new List<ColorEnum>();
        for (int i = 0; i < totalWires; i++)
        {
            wires.Add(GetRandomColor());
        }
        problem = wires;
    }

    public override void GenerateSolution()
    {
        int totalNumbers = 6;
        List<WireNumber> numbers = new List<WireNumber>();
        for (int i = 0; i < totalNumbers; i++)
        {
            WireNumber num = new WireNumber();
            num.color = GetRandomColor();
            num.number = rand.Next(51);
            num.isAnswer = false;
            numbers.Add(num);
        }


        if (totalWires == 4)
        {
            for (int i = 0; i < totalNumbers; i++)
            {
                if (numbers[i].color == wires[2])
                {
                    SetCorrectAnswer(numbers, i);
                }
            }
        }
        else if (wires[0] == ColorEnum.Green)
        {
            for (int i = 0; i < totalNumbers; i++)
            {
                if (numbers[i].number % 2 == 1)
                {
                    SetCorrectAnswer(numbers, i);
                }
            }
            numbers.Sort((x, y) => x.number.CompareTo(y.number));
        }
        else if (totalWires == 3)
        {
            int average = 0;
            for (int i = 0; i < totalNumbers; i++)
            {
                average += numbers[i].number;
            }
            average /= totalNumbers;

            List<double> distances = new List<double>();
            for (int i = 0; i < totalNumbers; i++)
            {
                double distance = numbers[i].number - average;
                distance = (distance < 0) ? -distance : distance;
                distances.Add(distance);
            }

            for (int i = 0; i < 3; i++)
            {
                double closest = 999;
                for (int j = 0; j < totalNumbers; j++)
                {
                    if (distances[j] < closest && !numbers[j].isAnswer) closest = distances[j];
                }
                int index = distances.FindIndex(x => x == closest);

                SetCorrectAnswer(numbers, index);
            }
        }
        else
        {
            int[] init = new int[Enum.GetValues(typeof(ColorEnum)).Length];
            List<int> totalWireColors = new List<int>(init);
            for (int i = 0; i < totalWires; i++)
            {
                //Debug.Log((int)wires[i]);
                totalWireColors[(int)wires[i]]++;
            }
            totalWireColors.Sort();
            int mostColor = totalWireColors[totalWireColors.Count - 1];

            for (int i = 0; i < totalNumbers; i++)
            {
                if (numbers[i].number % mostColor == 0)
                {
                    SetCorrectAnswer(numbers, i);
                }
            }
            numbers.Sort((x, y) => y.number.CompareTo(x.number));
        }
        solution = numbers;
    }

    public override void LinkUIToLogic()
    {
        // tampilan wire ada di problem
        // tampilan button ada di solution

        // problem -> List<ColorEnum>()
        // solution -> List<WireNumber>()

        // i need wire object references from the scene
        // i also need button references from the scene

        WireModuleComponents wmc = gameModule.GetComponent<WireModuleComponents>();

        List<ColorEnum> castedProblem = problem as List<ColorEnum>;
        float wireLeft = totalWires;
        int wireIdx = 0;
        for (int i = 0; i < wmc.wires.Count; i++)
        {
            Color color = new Color();
            if ((wireLeft / (wmc.wires.Count - i)) >= rand.NextDouble() && wireLeft != 0)
            {
                color = WireModuleComponents.colorDict[castedProblem[wireIdx]];
                wireLeft -= 1;
                wireIdx++;
            }
            else
            {
                color.a = 0;
            }

            wmc.wires[i].color = color;
        }

        List<WireNumber> castedSolution = solution as List<WireNumber>;
        List<Button> btnCpy = new List<Button>(wmc.buttons);
        List<TextMeshProUGUI> textCpy = new List<TextMeshProUGUI>(wmc.buttonTexts);

        foreach (Button btn in btnCpy)
        {
            btn.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < wmc.buttons.Count; i++)
        {
            int btnIdx = rand.Next(btnCpy.Count);

            btnCpy[btnIdx].image.color = WireModuleComponents.colorDict[castedSolution[i].color];
            textCpy[btnIdx].text = castedSolution[i].number.ToString();

            switch (castedSolution[i].color)
            {
                case ColorEnum.Red:
                    textCpy[btnIdx].color = Color.white;
                    break;
                case ColorEnum.Green:
                    textCpy[btnIdx].color = Color.black;
                    break;
                case ColorEnum.Blue:
                    textCpy[btnIdx].color = Color.white;
                    break;
                default:
                    break;
            }

            int idx = i;
            Button btn = btnCpy[btnIdx];
            btn.onClick.AddListener(delegate { btn.interactable = false; SubmitAnswer((solution as List<WireNumber>)[idx]); });

            btnCpy.RemoveAt(btnIdx);
            textCpy.RemoveAt(btnIdx);
        }
    }   

    public override void SubmitAnswer(object answer)
    {
        //hasCheckInCurrentFrame = false;
        Debug.Log($"SubmitAnswer, Answer: [{((WireNumber)answer).number}, {((WireNumber)answer).color}]");

        ((List<WireNumber>)this.answer).Add((WireNumber)answer);
        if (!((WireNumber)answer).isAnswer)
        {
            ResetAnswer();
        }
    }

    public override void ResetAnswer()
    {
        //hasCheckInCurrentFrame = false;
        answer = new List<WireNumber>();
        foreach (Button btn in gameModule.GetComponent<WireModuleComponents>().buttons)
        {
            btn.interactable = true;
        }
    }

    public override bool? CheckAnswer()
    {
        //if (hasCheckInCurrentFrame)
        //{
        //    return null;
        //}

        hasCheckInCurrentFrame = true;

        List<WireNumber> castedAnswer = answer as List<WireNumber>;
        List<WireNumber> castedSolution = solution as List<WireNumber>;

        // TODO: Remove the debug
        List<WireNumber> debugSolution = castedSolution.Where((wn) => wn.isAnswer).ToList();
        string debugString = "Solution: ";
        foreach (WireNumber solution in debugSolution)
        {
            debugString += $"[{solution.number}, {solution.color}]; ";
        }
        Debug.Log(debugString);

        if (castedAnswer.Count == 0)
        {
            return null;
        }

        if (!castedAnswer[castedAnswer.Count - 1].isAnswer)
        {
            ResetAnswer();
            return null;
        }

        string answerString = "Answer: ";
        foreach (var wn in castedAnswer)
        {
            answerString += $"[{wn.number}, {wn.color}]; ";
        }
        Debug.Log(answerString);
        
        // kalo kasus pertama (kabel ada 4)
        // berarti harus teken semua tombol yang warnanya sama dengan kabel di posisi ketiga
        if (totalWires == 4)
        {
            int currAnsCount = castedAnswer.Count;
            int solutionCount = castedSolution.Where((wn) => wn.isAnswer).ToList().Count;

            Debug.Log($"currAnsCount: {currAnsCount}, solutionCount: {solutionCount}, (currAnsCount == solutionCount) = {currAnsCount == solutionCount}");

            if (currAnsCount == solutionCount)
            {
                return true;
            }
        }

        else if (wires[0] == ColorEnum.Green)
        {
            int currAns = castedAnswer[castedAnswer.Count - 1].number;
            List<WireNumber> solutionList = castedSolution.Where((wn) => wn.isAnswer).ToList();

            if (solutionList[castedAnswer.Count - 1].number != currAns)
            {
                ResetAnswer();
            }

            else if (solutionList.Count == castedAnswer.Count)
            {
                return true;
            }
        }

        else if (totalWires == 3)
        {
            if (!castedAnswer[castedAnswer.Count - 1].isAnswer)
            {
                ResetAnswer();
                return null;
            }

            int solutionCount = castedSolution.Where((wn) => wn.isAnswer).Count();
            
            if (castedAnswer.Count == solutionCount)
            {
                return true;
            }
        }

        else
        {
            int currAns = castedAnswer[castedAnswer.Count - 1].number;
            List<WireNumber> solution = castedSolution.Where((wn) => wn.isAnswer).ToList();

            if (solution[castedAnswer.Count - 1].number != currAns)
            {
                ResetAnswer();
            }

            else if (solution.Count == castedAnswer.Count) 
            {
                return true;
            }
        }

        return null;
    }
}