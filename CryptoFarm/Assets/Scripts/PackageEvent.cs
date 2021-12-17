using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageEvent : BonusEvent
{
    public override void StartEvent()
    {
        if (gameObject.activeSelf)
            return;

        Debug.Log("Package event start");
        IsCurrentlyActive = true;
        gameObject.SetActive(true);
    }

    public override void EndEvent()
    {
        Debug.Log("Package event end");
        IsCurrentlyActive = false;
        gameObject.SetActive(false);
    }
}