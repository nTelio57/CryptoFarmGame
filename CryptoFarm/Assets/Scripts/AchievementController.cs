using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AchievementController : MonoBehaviour
{
    public AchievementPopup AchievementDetailPopup;
    public AchievementPopup AchievementCompletedPopup;
    private Animator AchievementCompletedPopupAnimator;
    public Dictionary<string, List<Achievement>> Achievements;
    
    public void Initiate()
    {
        AchievementCompletedPopupAnimator = AchievementCompletedPopup.GetComponent<Animator>();
        Achievements = new Dictionary<string, List<Achievement>>();
        GetAchievementsToDictionary();

        foreach (var achievement in GetComponentsInChildren<Achievement>())
        {
            achievement.SetController(this);
        }

    }

    private void GetAchievementsToDictionary()
    {
        var allAchievements = GetComponentsInChildren<Achievement>();
        var achievementIdsList = allAchievements.GroupBy(x => x.Id).Select(x => x.First()).ToList().Select(x => x.Id);
        foreach (string achievementId in achievementIdsList)
        {
            var newList = allAchievements.Where(x => x.Id.Equals(achievementId)).ToList();
            Achievements.Add(achievementId, newList);
        }
        
    }

    public void ShowAchievementDetails(Achievement achievement)
    {
        AchievementDetailPopup.SetAchievement(achievement);
        AchievementDetailPopup.Enable();
    }

    public void ShowAchievementCompleted(Achievement achievement)
    {
        AchievementCompletedPopup.SetAchievement(achievement);
        //AchievementCompletedPopup.Enable();
        AchievementCompletedPopupAnimator.SetTrigger("SetCompleted");
    }

    public List<Achievement> GetAchievementsById(string id)
    {
        return Achievements[id];
    }
}
