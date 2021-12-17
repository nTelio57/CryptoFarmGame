using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerGroup : MonoBehaviour
{
    public string Title;
    
    public int CurrentMinerPriority = -1;

    public Miner CurrentMiner;

    public void SetCurrentMiner(Miner miner)
    {
        CurrentMiner = miner;
        CurrentMinerPriority = miner.PriorityInGroup;
    }

    public void Reset()
    {
        CurrentMiner = null;
        CurrentMinerPriority = -1;
    }

}
