using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPopup : MonoBehaviour
{
    public float ActiveTime;

    public Text Title;
    public Text Description;
    public Slider Slider;
    public Text StateValue;
    public Text StateGoal;

    private float _timer;
    private bool _enabled;

    private void Update()
    {
        ActiveTimer();
    }

    private void ActiveTimer()
    {
        if (_enabled)
        {
            _timer += Time.deltaTime;
            if (_timer >= ActiveTime)
                Enable(false);
        }
    }

    public void Enable(bool enable = true)
    {
        if (enable)
        {
            _timer = 0;
            _enabled = true;
            gameObject.SetActive(true);
        }
        else
        {
            _enabled = false;
            gameObject.SetActive(false);
        }
    }

    public void SetAchievement(Achievement achievement)
    {
        Title.text = achievement.Title;
        Description.text = achievement.Description;
        Slider.maxValue = 1;
        Slider.value = GetProportionalValueByGoal(achievement.Value, achievement.Goal);
        StateValue.text = Utils.MoneyToString(achievement.Value);
        StateGoal.text = Utils.MoneyToString(achievement.Goal);
    }

    public IEnumerator Show(Achievement achievement)
    {
        Title.text = achievement.Title;
        Description.text = achievement.Description;
        Slider.maxValue = 1;
        Slider.value = GetProportionalValueByGoal(achievement.Value, achievement.Goal);
        StateValue.text = Utils.MoneyToString(achievement.Value);
        StateGoal.text = Utils.MoneyToString(achievement.Goal);

        gameObject.SetActive(true);

        yield return new WaitForSeconds(2);

        gameObject.SetActive(false);
    }

    private float GetProportionalValueByGoal(double value, double goal)
    {
        return (float)(value / goal);
    }
}
