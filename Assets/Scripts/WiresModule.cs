using System;
using System.Collections.Generic;

public class WiresModule : BaseModule
{
    Random rand = new Random();
    public struct WireNumber
    {
        public WireColorEnum color;
        public int number;
        public bool isAnswer;
    }

    public override void GenerateProblemAndSolution()
    {
        #region Generate Problem
        int totalWires = rand.Next(3, 6);
        List<WireColorEnum> wires = new List<WireColorEnum>();
        for (int i = 0; i < totalWires; i++)
        {
            wires.Add(GetRandomColor());
        }
        problem = wires;
        #endregion

        #region Generate Solution
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


        if(totalWires == 4)
        {
            for (int i = 0; i < totalNumbers; i++)
            {
                if(numbers[i].color == wires[2])
                {
                    SetCorrectAnswer(numbers[i]);
                }
            }
        }
        else if(wires[0] == WireColorEnum.Green)
        {
            for (int i = 0; i < totalNumbers; i++)
            {
                if(numbers[i].number % 2 == 1)
                {
                    SetCorrectAnswer(numbers[i]);
                }
            }
            numbers.Sort((x, y) => x.number.CompareTo(y.number));
        }
        else if(totalWires == 3)
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

            for(int i = 0; i < 3; i++)
            {
                double closest = 999;
                for(int j = 0; j < totalNumbers; j++)
                {
                    if (distances[j] < closest && !numbers[j].isAnswer) closest = distances[j];
                }
                int index = distances.FindIndex(x => x == closest);

                SetCorrectAnswer(numbers[index]);
            }
        }
        else
        {
            int[] init = new int[Enum.GetValues(typeof(WireColorEnum)).Length];
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
                if(numbers[i].number % mostColor == 0)
                {
                    SetCorrectAnswer(numbers[i]);
                }
            }
            numbers.Sort((x, y) => x.number.CompareTo(y.number));
        }
        solution = numbers;
        #endregion
    }

    private WireColorEnum GetRandomColor()
    {
        Array wireEnums = Enum.GetValues(typeof(WireColorEnum));
        return (WireColorEnum)wireEnums.GetValue(rand.Next(wireEnums.Length));
    }

    private void SetCorrectAnswer(WireNumber number)
    {
        WireNumber n = number;
        n.isAnswer = true;
        number = n;
    }
}