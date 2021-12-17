using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement : MonoBehaviour
{
    public string Id;
    public string Title;
    public string Description;
    public double Value;
    public double Goal;
    public bool IsCompleted;

    [Header("Reward")] 
    public BoosterItem Item;

    [Header("UI")]
    public Image ValueImage;

    private AchievementController _achievementController;

    private void Start()
    {
        _achievementController = GetComponentInParent<AchievementController>();
    }

    public void SetController(AchievementController controller)
    {
        _achievementController = controller;
    }

    public void OnClick()
    {
        _achievementController.ShowAchievementDetails(this);
    }

    public void Increment(double amount)
    {
        if (Value < Goal)
        {
            Value += amount;
            if (Value > Goal)
                Value = Goal;

            if (IsGoalReached() && !IsCompleted)
            {
                SetCompleted();
                _achievementController.ShowAchievementCompleted(this);
                Item.AddBooster();
            }
        }
    }

    public void SetCompleted(bool value=true)
    {
        IsCompleted = value;
    }

    public bool IsGoalReached()
    {
        return Value >= Goal;
    }
}
