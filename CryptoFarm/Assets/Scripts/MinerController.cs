using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinerController : MonoBehaviour
{
    public GameController GameController;
    [SerializeField]
    private Miner[] Miners;

    public double LocationRatio;

    public int MultiBuyValue = 1;

    public int LastRecognizedMiner;

    private void Start()
    {
        LocationRatio = GameController.GetLocationRatio();
        ResetUI();
    }

    public void Initiate()
    {
        foreach (var miner in Miners)
        {
            miner.SetController(this);
        }
    }

    public void SaveData()
    {
        SaveSystem.SaveMinerController(this);
    }

    public void LoadData()
    {
        var data = SaveSystem.LoadMinerController();
        if (data == null)
            return;
        for (int i = 0; i < data.Length; i++)
        {
            Miners[i].Amount = data.Amount[i];
            Miners[i].Price = data.Price[i];
            Miners[i].MiningPerSecond = data.MiningPerSecond[i];
            Miners[i].CurrentBoost = data.CurrentBoost[i];
            Miners[i].Recognized = data.Recognized[i];
            Miners[i].SetModelActive(data.IsModelActive[i]);

            if (Miners[i].IsModelActive && Miners[i].Group != null)
                Miners[i].Group.SetCurrentMiner(Miners[i]);
            
        }

        LastRecognizedMiner = data.LastRecognizedMiner;
    }

    public Miner[] GetMiners()
    {
        return Miners;
    }

    public void BuyMiner(Miner miner)
    {
        if (miner.CalculatePrice(MultiBuyValue) <= GameController.Money)
        {
            GameController.AddMoney(-miner.CalculatePrice(MultiBuyValue));

            if (miner.Amount == 0)//first miner of a kind
            {
                if (miner.Group != null)
                {
                    var group = miner.Group;
                    if (miner.PriorityInGroup >= group.CurrentMinerPriority)
                    {
                        if(group.CurrentMiner != null)
                            group.CurrentMiner.SetModelActive(false);
                        group.SetCurrentMiner(miner);
                        miner.SetModelActive(true);
                    }
                }
                else
                {
                    miner.SetModelActive(true);
                }
                
                if (miner != Miners[Miners.Length - 1] && 
                    Miners[LastRecognizedMiner + 1].LocationId <= GameController.LocationController.GetCurrentLocationId())//not last miner in shopping list
                {
                    LastRecognizedMiner++;
                    Miners[LastRecognizedMiner].Recognized = true;
                }
            }

            miner.Amount += MultiBuyValue;
            for (int i = 0; i < MultiBuyValue; i++)
            {
                miner.Price = miner.Price * miner.PriceMultiplier;

                miner.MiningPerSecond += miner.StartingMiningPerSecond;
            }

            miner.PriceText.text = $"{Utils.MoneyToString(miner.Price)}";
            miner.NextMiningPowerText.text = $"+{Utils.MoneyToString(miner.GetNextMiningPower() * MultiBuyValue)}/sec";
            miner.AmountText.text = $"x{miner.Amount}";

            GameController.UpdateMoneyPerSecond();
            GameController.UpdatePowerPerSecond();
            GameController.UpdateUI();
            ResetUI();
        }
    }

    public void CheckAvailableMiners()
    {
        var currentBalance = GameController.Money;
        foreach (var miner in Miners)
        {
            if (!miner.Recognized)
            {
                if (miner.Price * 0.5 <= currentBalance && miner.LocationId <= GameController.LocationController.GetCurrentLocationId())
                {
                    miner.Recognized = true;
                }

                else
                {
                    miner.gameObject.SetActive(false);
                }
                /*else if (!(miner.Price <= currentBalance))
                {
                    miner.gameObject.SetActive(false);
                }*/
            }
            else
            {
                if (miner.CalculatePrice(MultiBuyValue) <= currentBalance)
                {
                    miner.gameObject.SetActive(true);
                    miner.BuyButton.interactable = true;
                }
                else
                {
                    miner.gameObject.SetActive(true);
                    miner.BuyButton.interactable = false;
                }
            }
        }
    }

    public double GetLocationRatio()
    {
        return GameController.GetLocationRatio();
    }

    void ResetUI()
    {
        foreach (var miner in Miners)
        {
            miner.PriceText.text = $"{Utils.MoneyToString(miner.CalculatePrice(MultiBuyValue))}";
            miner.NextMiningPowerText.text = $"+{Utils.MoneyToString(miner.GetNextMiningPower() * MultiBuyValue)}/sec";
            miner.AmountText.text = $"x{miner.Amount}";
            UpdateMiningPowerUI();
        }
    }

    void UpdateMiningPowerUI()
    {
        foreach (var miner in Miners)
        {
            miner.UpdateMiningPowerUI(GameController.MoneyPerSecond);
            /*if(GameController.MoneyPerSecond > 0)
                miner.MiningPowerText.text = $"{Utils.MoneyToString(miner.GetMiningPerSecond())}/sec ({(miner.GetMiningPerSecond() * 100 / GameController.MoneyPerSecond):N1}%)";
            else
                miner.MiningPowerText.text = $"{Utils.MoneyToString(miner.GetMiningPerSecond())}/sec (0%)";*/
        }
    }

    public void OnMultiBuyClick(int value)
    {
        MultiBuyValue = value;
        ResetUI();
    }

    public void Reset()
    {
        foreach (var miner in Miners)
        {
            miner.Amount = 0;
            miner.Price = miner.StartingPrice;
            miner.MiningPerSecond = 0;
            miner.Recognized = false;
            miner.SetModelActive(false);
            if(miner.Group != null)
                miner.Group.Reset();
        }
        Miners[0].Recognized = true;
        ResetUI();
        LastRecognizedMiner = 0;
    }

}
