using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataPowerController 
{
    public int LastRecognizedPowerGenerator;

    public int[] Amount;
    public double[] Price;
    public double[] PowerPerSecond;
    public float[] CurrentBoost;
    public bool[] Recognized;
    public bool[] IsModelActive;
    public int Length;

    public DataPowerController(PowerController powerController)
    {
        LastRecognizedPowerGenerator = powerController.LastRecognizedPowerGenerator;

        var powerGenerators = powerController.GetPowerGenerators();
        var n = powerGenerators.Length;

        Length = n;

        Amount = new int[n];
        Price = new double[n];
        PowerPerSecond = new double[n];
        CurrentBoost = new float[n];
        Recognized = new bool[n];
        IsModelActive = new bool[n];

        for (int i = 0; i < n; i++)
        {
            Amount[i] = powerGenerators[i].Amount;
            Price[i] = powerGenerators[i].Price;
            PowerPerSecond[i] = powerGenerators[i].PowerPerSecond;
            CurrentBoost[i] = powerGenerators[i].CurrentBoost;
            Recognized[i] = powerGenerators[i].Recognized;
            IsModelActive[i] = powerGenerators[i].IsModelActive;
        }
    }
}
