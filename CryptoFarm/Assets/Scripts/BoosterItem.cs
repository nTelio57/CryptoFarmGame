using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BoostType
{
    None,
    ReduceTotalEnergy,
    GetAdditionalProfitFromMiners,
    IncreaseChanceForItemDrop,
    IncreaseEventReward
}

public class 
    BoosterItem : MonoBehaviour
{
    public string Name;
    public string Description;
    public double Price;
    public int Amount;
    public CurrencyType Currency;
    public float Boost;
    public Miner Miner;
    public bool Owned;
    public bool Recognized;

    [Header("Boost")]
    public BoostType BoostType;
    public float BoostAmount;

    [Header("UI")] 
    public Text NameText;
    public Text PriceText;
    public Text DescriptionText;
    public Button BuyButton;

    private BoosterController _boosterController;

    public void SetController(BoosterController controller)
    {
        _boosterController = controller;
    }

    public void OnBuyClick()
    {
        _boosterController.BuyBooster(this);
    }

    public void AddBooster()
    {
        _boosterController.AddBooster(this);
    }
}
