using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerController : MonoBehaviour
{
    [SerializeField]
    private GameController GameController;
    [SerializeField]
    private PowerGenerator[] PowerGenerators;

    public int MultiBuyValue = 1;

    public int LastRecognizedPowerGenerator;

    private void Start()
    {
        ResetUI();
    }

    public void SaveData()
    {
        SaveSystem.SavePowerController(this);
    }

    public void LoadData()
    {
        var data = SaveSystem.LoadPowerController();
        if (data == null)
            return;
        for (int i = 0; i < data.Length; i++)
        {
            PowerGenerators[i].Amount = data.Amount[i];
            PowerGenerators[i].Price = data.Price[i];
            PowerGenerators[i].PowerPerSecond = data.PowerPerSecond[i];
            PowerGenerators[i].CurrentBoost = data.CurrentBoost[i];
            PowerGenerators[i].Recognized = data.Recognized[i];
            PowerGenerators[i].SetModelActive(data.IsModelActive[i]);
        }

        LastRecognizedPowerGenerator = data.LastRecognizedPowerGenerator;
    }

    public PowerGenerator[] GetPowerGenerators()
    {
        return PowerGenerators;
    }

    public void OnMultiBuyClick(int value)
    {
        MultiBuyValue = value;
        ResetUI();
    }

    void ResetUI()
    {
        foreach (var powerGenerator in PowerGenerators)
        {
            powerGenerator.PriceText.text = $"{Utils.MoneyToString(powerGenerator.CalculatePrice(MultiBuyValue))}";
            powerGenerator.NextPowerText.text =
                $"+{Utils.MoneyToString(powerGenerator.GetNextPower() * MultiBuyValue)}/sec";
            powerGenerator.AmountText.text = $"x{powerGenerator.Amount}";
            powerGenerator.CurrentPowerText.text = $"{Utils.MoneyToString(powerGenerator.GetPowerPerSecond())}/sec";
        }
    }

    public void CheckAvailablePowerGenerators()
    {
        var currentBalance = GameController.Money;
        foreach (var powerGenerator in PowerGenerators)
        {
            if (!powerGenerator.Recognized)
            {
                if (powerGenerator.Price * 0.5 <= currentBalance && powerGenerator.LocationId <= GameController.LocationController.GetCurrentLocationId())
                {
                    powerGenerator.Recognized = true;
                }
                else
                {
                    powerGenerator.gameObject.SetActive(false);
                }
                /*else if (!(powerGenerator.Price <= currentBalance))
                {
                    powerGenerator.gameObject.SetActive(false);
                }*/
            }
            else
            {
                if (powerGenerator.CalculatePrice(MultiBuyValue) <= currentBalance)
                {
                    powerGenerator.gameObject.SetActive(true);
                    powerGenerator.BuyButton.interactable = true;
                }
                else
                {
                    powerGenerator.gameObject.SetActive(true);
                    powerGenerator.BuyButton.interactable = false;
                }
            }
        }
    }

    public void BuyPowerGenerator(PowerGenerator powerGenerator)
    {
        if (powerGenerator.CalculatePrice(MultiBuyValue) <= GameController.Money)
        {
            GameController.AddMoney(-powerGenerator.CalculatePrice(MultiBuyValue));

            if (powerGenerator.Amount == 0)
            {
                powerGenerator.SetModelActive(true);

                if (powerGenerator != PowerGenerators[PowerGenerators.Length - 1] &&
                    PowerGenerators[LastRecognizedPowerGenerator + 1].LocationId <= GameController.LocationController.GetCurrentLocationId())
                {
                    LastRecognizedPowerGenerator++;
                    PowerGenerators[LastRecognizedPowerGenerator].Recognized = true;
                }
            }

            powerGenerator.Amount += MultiBuyValue;
            for (int i = 0; i < MultiBuyValue; i++)
            {
                powerGenerator.Price = powerGenerator.Price * powerGenerator.PriceMultiplier;

                powerGenerator.PowerPerSecond += powerGenerator.StartingPowerPerSecond;
            }

            powerGenerator.PriceText.text = $"{Utils.MoneyToString(powerGenerator.Price)}";
            powerGenerator.NextPowerText.text = $"+{Utils.MoneyToString(powerGenerator.GetNextPower() * MultiBuyValue)}/sec";
            powerGenerator.AmountText.text = $"x{powerGenerator.Amount}";

            GameController.UpdateMoneyPerSecond();
            GameController.UpdatePowerPerSecond();
            GameController.UpdateUI();
            ResetUI();
        }
    }

    public void Reset()
    {
        foreach (var powerGenerator in PowerGenerators)
        {
            powerGenerator.Amount = 0;
            powerGenerator.Price = powerGenerator.StartingPrice;
            powerGenerator.PowerPerSecond = 0;
            powerGenerator.Recognized = false;
            powerGenerator.SetModelActive(false);
        }
        PowerGenerators[0].Recognized = true;
        ResetUI();
        LastRecognizedPowerGenerator = 0;
    }
}
