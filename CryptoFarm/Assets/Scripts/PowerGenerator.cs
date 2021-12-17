using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerGenerator : MonoBehaviour
{
    public string Name;
    public int Amount;
    public double Price;
    public double StartingPrice;
    public float PriceMultiplier;
    public double PowerPerSecond;
    public double StartingPowerPerSecond;
    public float CurrentBoost;
    public bool Recognized;
    public int LocationId;

    [Header("Models")]
    public GameObject[] Models;
    public bool IsModelActive;

    [Header("UI")]
    public Text TitleText;
    public Text PriceText;
    public Text NextPowerText;
    public Text CurrentPowerText;
    public Text AmountText;
    public Button BuyButton;

    private PowerController _powerController;

    private void Start()
    {
        _powerController = GetComponentInParent<PowerController>();
        TitleText.text = Name;
    }

    public void OnBuyClick()
    {
        _powerController.BuyPowerGenerator(this);
    }

    public double GetNextPower()
    {
        return StartingPowerPerSecond + StartingPowerPerSecond * CurrentBoost;
    }

    public double GetPowerPerSecond()
    {
        return PowerPerSecond + PowerPerSecond * CurrentBoost;
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
