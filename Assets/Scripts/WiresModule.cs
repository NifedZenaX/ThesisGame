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
    int totalButtons;
    
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

    private void SetCorrectAnswer(List<WireNumber> numbers, int index, int number)
    {
        WireNumber n = numbers[index];
        n.number = number;
        n.isAnswer = true;
        numbers[index] = n;
    }

    private void SetCorrectAnswer(List<WireNumber> numbers, int index)
    {
        SetCorrectAnswer(numbers, index, numbers[index].number);
    }

    public override void GenerateProblem()
    {
        totalWires = rand.Next(2, 6);
        wires = new List<ColorEnum>();
        for (int i = 0; i < totalWires; i++)
        {
            wires.Add(GetRandomColor());
        }
        problem = wires;
    }

    public override void GenerateSolution()
    {
        Random rand = new Random();
        totalButtons = rand.Next(3, 7);

        List<WireNumber> numbers = new List<WireNumber>();
        for (int i = 0; i < totalButtons; i++)
        {
            WireNumber num = new WireNumber();
            num.color = GetRandomColor();
            num.number = 0;
            num.isAnswer = false;
            numbers.Add(num);
        }

        if (totalWires == 2)
        {
            WireNumber wn = numbers[0];
            wn.number = rand.Next(1, 26) * 2;
            numbers[0] = wn;
            SetCorrectAnswer(numbers, 0);

            for (int i = 1; i < totalButtons; i++)
            {
                wn = numbers[i];
                wn.number = rand.Next(1, 51);
                numbers[i] = wn;
                
                if (numbers[i].number % 2 == 0)
                {
                    SetCorrectAnswer(numbers, i);
                }
            }

            numbers.Sort((x, y) => y.number.CompareTo(x.number));
        }
        
        else if (totalWires == 3)
        {
            int maxButtonValue = 10;
            int average = rand.Next(3, maxButtonValue + 1);
            int total = totalButtons * average;

            WireNumber correctAns = numbers[0];
            correctAns.number = average;
            numbers[0] = correctAns;
            SetCorrectAnswer(numbers, 0);

            int totalValue = numbers[0].number;

            for (int i = 1; i < totalButtons; i++)
            {
                int slotLeft = totalButtons - i;
                int slotTotalValue = total - totalValue;

                int minRange = (slotTotalValue > ((slotLeft - 1) * maxButtonValue)) ? slotTotalValue % ((slotLeft > 1) ? (slotLeft - 1) * maxButtonValue : maxButtonValue) : 1;
                if (minRange == 0)
                {
                    minRange = maxButtonValue;
                }

                int maxRange = slotTotalValue - ((slotLeft - 1) * minRange);
                if (maxRange >= maxButtonValue)
                {
                    maxRange = maxButtonValue - 1;
                }

                Debug.Log($"totalValue: {totalValue}, slotTotalValue: {slotTotalValue}, average: {average}, minRange: {minRange}, maxRange: {maxRange}");

                int num = 0;
                do
                {
                    num = rand.Next(minRange, maxRange + 1);
                }
                while (num == 0 || num == average);

                totalValue += num;

                WireNumber n = numbers[i];
                n.number = num;
                numbers[i] = n;

                if (num == average)
                {
                    SetCorrectAnswer(numbers, i);
                }

                Debug.Log($"index: {i}, number: {n.number}");
            }
        }

        else if (totalWires == 4)
        {
            WireNumber correctAnswer = numbers[0];
            correctAnswer.number = (rand.Next(1, 25) * 2) - 1;
            numbers[0] = correctAnswer;
            SetCorrectAnswer(numbers, 0);

            for (int i = 1; i < totalButtons; i++)
            {
                WireNumber wn = numbers[i];
                wn.number = rand.Next(1, 51);
                numbers[i] = wn;

                if (numbers[i].number % 2 == 1)
                {
                    SetCorrectAnswer(numbers, i);
                }
            }
            numbers.Sort((x, y) => x.number.CompareTo(y.number));
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

            WireNumber wn = numbers[0];
            wn.number = mostColor * rand.Next(1, 11);
            numbers[0] = wn;
            SetCorrectAnswer(numbers, 0);

            for (int i = 1; i < totalButtons; i++)
            {
                wn = numbers[i];
                wn.number = rand.Next(1, 51);
                numbers[i] = wn;
                if (numbers[i].number % mostColor == 0)
                {
                    SetCorrectAnswer(numbers, i);
                }
            }
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
            btn.gameObject.SetActive(false);
        }

        for (int i = 0; i < totalButtons; i++)
        {
            int btnIdx = rand.Next(btnCpy.Count);

            btnCpy[btnIdx].image.color = Color.white;
            btnCpy[btnIdx].gameObject.SetActive(true);
            textCpy[btnIdx].text = castedSolution[i].number.ToString();
            
            switch (castedSolution[i].color)
            {
                case ColorEnum.Red:
                    textCpy[btnIdx].color = Color.black;
                    break;
                case ColorEnum.Green:
                    textCpy[btnIdx].color = Color.black;
                    break;
                case ColorEnum.Blue:
                    textCpy[btnIdx].color = Color.black;
                    break;
                default:
                    break;
            }

            int idx = i;
            Button btn = btnCpy[btnIdx];
            if (btn.gameObject.activeInHierarchy)
            {
                btn.onClick.AddListener(delegate { btn.interactable = false; 
                    Console.WriteLine("Before SubmitAnswer"); 
                    SubmitAnswer(((List<WireNumber>)solution)[idx]); 
                    Console.WriteLine("After SubmitAnswer"); 
                    Debug.Log($"btn.onClick.AddListener:\nindex: {idx}"); 
                });
            }

            btnCpy.RemoveAt(btnIdx);
            textCpy.RemoveAt(btnIdx);
        }
    }   

    public override void SubmitAnswer(object answer)
    {
        Debug.Log("Submit Answer");
        hasCheckInCurrentFrame = false;

        ((List<WireNumber>)this.answer).Add((WireNumber)answer);
        if (!((WireNumber)answer).isAnswer)
        {
            Debug.Log("di submit answer bkn jawabannya, mulai masuk ke reset answer");
            ResetAnswer();
        }

    }

    public override void ResetAnswer()
    {
        Debug.Log("Reset Answer");
        hasCheckInCurrentFrame = false;

        answer = new List<WireNumber>();
        Debug.Log("tombol interactable mulai dinyalain");
        foreach (Button btn in gameModule.GetComponent<WireModuleComponents>().buttons)
        {
            btn.interactable = true;
        }
        Debug.Log("tombol interactable udah dinyalain");

    }

    public override bool? CheckAnswer()
    {
        Debug.Log("Check Answer");
        if (hasCheckInCurrentFrame)
        {
            Debug.Log("udah ngecek, nggak perlu dicek dulu jawabannya");
            return null;
        }
        hasCheckInCurrentFrame = true;


        List<WireNumber> castedAnswer = answer as List<WireNumber>;
        List<WireNumber> castedSolution = solution as List<WireNumber>;

        if (castedAnswer.Count == 0)
        {
            Debug.Log("jawaban kosong");
            return null;
        }

        if (!castedAnswer[castedAnswer.Count - 1].isAnswer)
        {
            Debug.Log("check answer general jawaban salah");
            ResetAnswer();
            return null;
        }

        // kalo kasus pertama (kabel ada 4)
        // berarti harus teken semua tombol yang warnanya sama dengan kabel di posisi ketiga
        //if (totalWires == 4)
        //{
        //    int currAnsCount = castedAnswer.Count;
        //    int solutionCount = castedSolution.Where((wn) => wn.isAnswer).Count();

        //    if (currAnsCount == solutionCount)
        //    {
        //        return true;
        //    }
        //}

        if (totalWires == 4 || totalWires == 2)
        {
            Debug.Log("check answer total wires 4 || 2");
            int currAns = castedAnswer[castedAnswer.Count - 1].number;
            List<WireNumber> solutionList = castedSolution.Where((wn) => wn.isAnswer).ToList();

            if (solutionList[castedAnswer.Count - 1].number != currAns)
            {
                Debug.Log("currAns bukan urutan yang bener, reset");
                ResetAnswer();
                return null;
            }

            else if (solutionList.Count == castedAnswer.Count)
            {
                Debug.Log("totalwires 4 || 2 jawaban bener");
                return true;
            }
        }

        else if (totalWires == 3)
        {
            Debug.Log("check answer total wires 3");

            if (!castedAnswer[castedAnswer.Count - 1].isAnswer)
            {
                Debug.Log("currAns bukan urutan yang bener, reset");
                ResetAnswer();
                return null;
            }

            int solutionCount = castedSolution.Where((wn) => wn.isAnswer).Count();
            
            if (castedAnswer.Count == solutionCount)
            {
                Debug.Log("totalwires 3 jawaban bener");
                return true;
            }
        }

        else
        {
            Debug.Log("check answer total wires 5");

            if (!castedAnswer[castedAnswer.Count - 1].isAnswer)
            {
                Debug.Log("totalwires 5 jawaban salah");
                ResetAnswer();
                return null;
            }

            List<WireNumber> solution = castedSolution.Where((wn) => wn.isAnswer).ToList();

            if (solution.Count == castedAnswer.Count) 
            {
                Debug.Log("totalwires 3 jawaban bener");
                return true;
            }
        }

        return null;
    }
}