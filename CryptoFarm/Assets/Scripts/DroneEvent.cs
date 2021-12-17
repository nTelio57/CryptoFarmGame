using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneEvent : BonusEvent
{
    public Animator Animator;
    public GameObject Drone;
    public override void StartEvent()
    {
        if (IsCurrentlyActive)
            return;
        
        IsCurrentlyActive = true;
        Animator.SetTrigger("StartEvent");
        Drone.SetActive(true);
    }

    public override void EndEvent()
    {
        IsCurrentlyActive = false;
        Animator.SetTrigger("EndEvent");
        Drone.SetActive(false);
    }
}
