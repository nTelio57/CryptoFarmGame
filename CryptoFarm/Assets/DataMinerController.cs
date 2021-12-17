using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataMinerController
{
    public int LastRecognizedMiner;

    public int[] Amount;
    public double[] Price;
    public double[] MiningPerSecond;
    public float[] CurrentBoost;
    public bool[] Recognized;
    public bool[] IsModelActive;
    public int Length;

    public DataMinerController(MinerController minerController)
    {
        LastRecognizedMiner = minerController.LastRecognizedMiner;

        var miners = minerController.GetMiners();
        var n = miners.Length;

        Length = n;

        Amount = new int[n];
        Price = new double[n];
        MiningPerSecond = new double[n];
        CurrentBoost = new float[n];
        Recognized = new bool[n];
        IsModelActive = new bool[n];

        for (int i = 0; i < n; i++)
        {
            Amount[i] = miners[i].Amount;
            Price[i] = miners[i].Price;
            MiningPerSecond[i] = miners[i].MiningPerSecond;
            CurrentBoost[i] = miners[i].CurrentBoost;
            Recognized[i] = miners[i].Recognized;
            IsModelActive[i] = miners[i].IsModelActive;
        }
    }
}
