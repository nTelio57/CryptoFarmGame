using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataGameController
{
    public int CurrentLocationId;
    public double Money;
    public double MoneyPerSecond;
    public double PowerPerSecond;
    public DataGameController(GameController gameController)
    {
        CurrentLocationId = gameController.LocationController.GetCurrentLocationId();
        Money = gameController.Money;
        MoneyPerSecond = gameController.MoneyPerSecond;
        PowerPerSecond = gameController.PowerPerSecond;
    }

}
