using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigEvent : BonusEvent
{
    public GameObject SmokeParticles;

    public GameObject[] SparkleParticles;
    public Animator[] GPUAnimator;

    public override void StartEvent()
    {
        if (IsCurrentlyActive)
            return;
        
        IsCurrentlyActive = true;

        var eventType = Random.Range(0, 2);
        if (eventType == 0)
        {
            StartCrash();
        }
        else
        {
            StartOverheat();
        }
    }

    public override void EndEvent()
    {
        IsCurrentlyActive = false;
        EndCrash();
        EndOverheat();
    }

    void StartCrash()
    {
        for (int i = 0; i < SparkleParticles.Length; i++)
        {
            SparkleParticles[i].SetActive(true);
            GPUAnimator[i].enabled = false;
        }
    }

    void EndCrash()
    {
        for (int i = 0; i < SparkleParticles.Length; i++)
        {
            SparkleParticles[i].SetActive(false);
            GPUAnimator[i].enabled = true;
        }
    }

    void StartOverheat()
    {
        SmokeParticles.SetActive(true);
    }

    void EndOverheat()
    {
        SmokeParticles.SetActive(false);
    }

    
}
