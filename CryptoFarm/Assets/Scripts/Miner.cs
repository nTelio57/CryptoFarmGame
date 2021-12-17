using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Miner : MonoBehaviour
{
    public string Name;
    public string Description;
    public int Amount;
    public double Price;
    public double PowerUsage;
    public float PowerInefficiency;
    public double StartingPrice;
    public float PriceMultiplier;
    public double MiningPerSecond;
    public double StartingMiningPerSecond;
    public float CurrentBoost;
    public bool Recognized;
    public int LocationId;
    
    [Header("Group")] 
    public MinerGroup Group;
    public int PriorityInGroup;
    public GameObject[] Models;
    public bool IsModelActive;

    [Header("UI")]
    public Text TitleText;
    public Text PriceText;
    public Text NextMiningPowerText;
    public Text MiningPowerText;
    public Text AmountText;
    public Button BuyButton;

    private MinerController _minerController;

    private void Awake()
    {
        TitleText.text = Name;
        PowerUsage = StartingMiningPerSecond * -PowerInefficiency;
        Price = StartingPrice;
    }

    public void SetController(MinerController controller)
    {
        _minerController = controller;
    }

    public void OnBuyClick()
    {
        _minerController.BuyMiner(this);
    }

    public void AddBoost(float amount)
    {
        CurrentBoost += amount;
    }

    public double GetMiningPerSecond()
    {
        var locationRatio = _minerController.GetLocationRatio();
        return (MiningPerSecond + MiningPerSecond * CurrentBoost) * locationRatio;
    }

    public double GetPowerPerSecond()
    {
        return PowerUsage * Amount;
    }

    public double GetNextMiningPower()
    {
        var locationRatio = _minerController.GetLocationRatio();
        return (StartingMiningPerSecond + StartingMiningPerSecond * CurrentBoost) * locationRatio;
    }

    public void ResetUI()
    {       
        PriceText.text = $"{Utils.MoneyToString(Price)}";
        NextMiningPowerText.text = $"+{Utils.MoneyToString(GetNextMiningPower())}/sec";
        AmountText.text = $"x{Amount}";
    }

    public void UpdateMiningPowerUI(double globalMoneyPerSecond)
    {
        if (globalMoneyPerSecond > 0)
            MiningPowerText.text = $"{Utils.MoneyToString(GetMiningPerSecond())}/sec ({(GetMiningPerSecond() * 100 / globalMoneyPerSecond):N1}%)";
        else
            MiningPowerText.text = $"{Utils.MoneyToString(GetMiningPerSecond())}/sec (0%)";
    }

    public double CalculatePrice(int times)
    {
        double sum = 0;
        var tempPrice = Price;
        for (int i = 0; i < times; i++)
        {
            sum += tempPrice;
            tempPrice *= PriceMultiplier;
        }

        return sum;
    }

    public void SetModelActive(bool value)
    {
        foreach (var model in Models)
        {
            model.SetActive(value);
        }

        IsModelActive = value;
    }
}
