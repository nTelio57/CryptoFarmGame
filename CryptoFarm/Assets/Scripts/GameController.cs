using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class GameController : MonoBehaviour
{
    public MinerController MinerController;
    public BoosterController BoosterController;
    public PowerController PowerController;
    public AchievementController AchievementController;
    public LocationController LocationController;

    public float AutoSaveTimer;
    public GameObject CameraParent;

    [Header("UI")]
    public Text MoneyText;
    public Text MoneyPerSecondText;
    public Text PowerPerSecondText;
    public GameObject CPSBonusTextObject;
    public Text CPSBonusText;

    [Header("Money")]
    public double Money;
    public double MoneyPerSecond;

    [Header("Power")]
    public double PowerPerSecond;

    [Header("Cps")] 
    public float CPSBoost;
    public float CPS;
    public float MinCPS;
    public float MaxCPS;
    public float CPSIncreasePerClick;
    public float CPSDecreasePerSecond;
    public float CPSTimeUntilDecrease;
    public float CpsBoostRequiredForUI;
    private float _noClickTimer;

    private void Awake()
    {
        LoadData();
        UpdateUI();
        MinerController.Initiate();
        BoosterController.Initiate();
        AchievementController.Initiate();

        StartCoroutine(AutoSave());
    }

    private void Update()
    {
        CalculateCPS();
        AddMoney(((GetMoneyPerSecond() + GetPowerPerSecond()) * CPSBoost) * Time.deltaTime);
        UpdateUI();
    }

    IEnumerator AutoSave()
    {
        while (true)
        {
            SaveData();
            MinerController.SaveData();
            PowerController.SaveData();
            yield return new WaitForSeconds(AutoSaveTimer);
        }
    }

    public void SaveData()
    {
        SaveSystem.SaveGameController(this);
    }

    public void LoadData()
    {
        var data = SaveSystem.LoadGameController();
        if (data == null)
            return;

        LocationController.SetLocation(data.CurrentLocationId);
        Money = data.Money;
        MoneyPerSecond = data.MoneyPerSecond;
        PowerPerSecond = data.PowerPerSecond;

        MinerController.LoadData();
        PowerController.LoadData();
    }

    public void AddMoney(double money)
    {
        Money += money;
        BoosterController.CheckAvailableBoosters();
        MinerController.CheckAvailableMiners();
        PowerController.CheckAvailablePowerGenerators();

        if (money > 0)
        {
            AchievementController.GetAchievementsById("moneyTotal").ForEach(x => x.Increment(money));
        }
    }

    public void UpdateUI()
    {
        MoneyText.text = Utils.MoneyToString(Money);
        MoneyPerSecondText.text = Utils.MoneyToString((GetMoneyPerSecond() + GetPowerPerSecond()) * CPSBoost) + "/sec ";
        PowerPerSecondText.text = Utils.MoneyToString(GetPowerPerSecond()) + "/sec";

        UpdateCPSUI();
    }

    private void UpdateCPSUI()
    {
        if(CPSBoost >= CpsBoostRequiredForUI && !CPSBonusTextObject.activeSelf)
            CPSBonusTextObject.SetActive(true);
        else if(CPSBoost < CpsBoostRequiredForUI && CPSBonusTextObject.activeSelf)
            CPSBonusTextObject.SetActive(false);

        if (CPSBonusTextObject.activeSelf)
            CPSBonusText.text = "x"+ CPSBoost.ToString("F1");
    }

    private double GetMoneyPerSecond()
    {
        return MoneyPerSecond * (1 + BoosterController.MinersProfitBoost);
    }

    private double GetPowerPerSecond()
    {
        return PowerPerSecond * (1 - BoosterController.ReduceTotalEnergyCost);
    }

    public void UpdateMoneyPerSecond()
    {
        MoneyPerSecond = MinerController.GetMiners().Where(x => x.Amount > 0).Sum(miner => miner.GetMiningPerSecond());
    }

    public void UpdatePowerPerSecond()
    {
        var profit = PowerController.GetPowerGenerators().Where(x => x.Amount > 0)
            .Sum(powerGenerator => powerGenerator.GetPowerPerSecond());
        var losses = MinerController.GetMiners().Where(x => x.Amount > 0).Sum(miner => miner.GetPowerPerSecond());
        PowerPerSecond = profit + losses;
    }

    void CalculateCPS()
    {
        _noClickTimer += Time.deltaTime;

        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if(CPS < MaxCPS)
                    CPS += CPSIncreasePerClick;
                _noClickTimer = 0;
            }
        }

        if (_noClickTimer >= CPSTimeUntilDecrease && CPS >= MinCPS)
        {
            CPS -= CPSDecreasePerSecond * Time.deltaTime;
        }

        CPSBoost = Mathf.Clamp(CPS, MinCPS, MaxCPS);

    }

    public void Reset()
    {
        MoneyPerSecond = 0;
        PowerPerSecond = 0;
        Money = 3;//neturetu but hardcode
        CPS = 0;
        CPSBoost = 0;

        MinerController.Reset();
        PowerController.Reset();
    }

    public void SetLocationRatio(double ratio)
    {
        MinerController.LocationRatio = ratio;
    }

    public double GetLocationRatio()
    {
        return LocationController.GetCurrentLocationRatio();
    }

    public void SetCameraPosition(Vector3 position)
    {
        CameraParent.transform.position = position;
    }
}
