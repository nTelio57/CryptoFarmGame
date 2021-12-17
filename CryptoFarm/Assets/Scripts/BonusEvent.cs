using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusEvent : MonoBehaviour
{
    public float RewardMultiplier;
    public float Chance;
    public bool IsCurrentlyActive;

    public virtual void StartEvent()
    {
        Debug.Log("Parent event start");
    }

    public virtual void EndEvent()
    {
        Debug.Log("Parent event end");
    }
}
