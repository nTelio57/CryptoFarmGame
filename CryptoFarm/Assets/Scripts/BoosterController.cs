using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterController : MonoBehaviour
{
    [SerializeField]
    private GameController GameController;
    [SerializeField]
    private BoosterItem[] Boosters;

    public float ReduceTotalEnergyCost;
    public float MinersProfitBoost;

    private void Start()
    {
        ResetUI();
    }

    public void Initiate()
    {
        foreach (var booster in Boosters)
        {
            booster.SetController(this);
        }
    }

    public void BuyBooster(BoosterItem booster)
    {
        AddBooster(booster);
        return;//temporary

        if (booster.Price <= GameController.Money)
        {
            GameController.AddMoney(-booster.Price);
            
            booster.Owned = true;

            GameController.UpdateMoneyPerSecond();
            GameController.UpdatePowerPerSecond();
            GameController.UpdateUI();

            booster.Miner.UpdateMiningPowerUI(GameController.MoneyPerSecond);
            booster.Miner.ResetUI();

            AddBoost(booster);
        }
    }

    public void AddBooster(BoosterItem booster)
    {
        booster.Amount += 1;
        AddBoost(booster);
    }

    private void AddBoost(BoosterItem booster)
    {
        switch (booster.BoostType)
        {
            case BoostType.ReduceTotalEnergy:
                AddTotalEnergyReductionPercentage(booster.BoostAmount);
                break;
            case BoostType.GetAdditionalProfitFromMiners:
                AddMinersProfitBoost(booster.BoostAmount);
                break;
        }
    }

    public void CheckAvailableBoosters()
    {
        var currentBalance = GameController.Money;
        foreach (var booster in Boosters)
        {
            if (booster.Owned)
            {
                booster.gameObject.SetActive(false);
            }
            else if (!booster.Recognized)
            {
                if (booster.Price * 0.5 <= currentBalance)
                {
                    booster.Recognized = true;
                }
                else if(!(booster.Price <= currentBalance))
                {
                    booster.gameObject.SetActive(false);
                }
            }
            else
            {
                if (booster.Price <= currentBalance)
                {
                    booster.gameObject.SetActive(true);
                    booster.BuyButton.interactable = true;
                }
                else
                {
                    booster.gameObject.SetActive(true);
                    booster.BuyButton.interactable = false;
                }
            }
        }
    }

    void ResetUI()
    {
        foreach (var booster in Boosters)
        {
            booster.PriceText.text = $"{Utils.MoneyToString(booster.Price)}";
            booster.DescriptionText.text = booster.Description;

            switch (booster.BoostType)
            {
                case BoostType.ReduceTotalEnergy:
                    booster.DescriptionText.text = "Reduces the total cost of energy consumed by miners";
                    break;
                case BoostType.GetAdditionalProfitFromMiners:
                    booster.DescriptionText.text = $"Extra profit from miners by {booster.BoostAmount * 100}%";
                    break;
            }
        }
    }

    public void AddTotalEnergyReductionPercentage(float amount)
    {
        ReduceTotalEnergyCost += amount;
    }

    public void AddMinersProfitBoost(float amount)
    {
        MinersProfitBoost += amount;
    }
}
