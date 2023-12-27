using System;
using System.Collections.Generic;

public class WiresModule : BaseModule
{
    public override void GenerateProblemAndSolution()
    {
        #region Generate Problem
        Random rand = new Random();
        int totalWires = rand.Next(1, 5);
        List<WireColorEnum> wires = new List<WireColorEnum>();

        Array wireEnums = Enum.GetValues(typeof(WireColorEnum));
        for (int i = 0; i < totalWires; i++)
        {
            wires.Add((WireColorEnum)wireEnums.GetValue(rand.Next(wireEnums.Length)));
        }
        #endregion

        #region Generate Solution

        #endregion
    }
}
