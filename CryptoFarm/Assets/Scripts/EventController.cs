using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EventController : MonoBehaviour
{
    public GameController GameController;
    public BonusEventRewardText RewardTextPrefab;
    public Transform RewardTextParent;
    public BonusEvent[] Events;
    
    public Vector2 MinPosition;//galima padaryti private ir nuskaityti pagal reward text prefabo staciakampio plotis / 2
    public Vector2 MaxPosition;

    private void Start()
    {

        StartCoroutine(EventTimer());
    }
    
    void Update()
    {
        DetectTouch();
    }

    private IEnumerator EventTimer()
    {
        while (true)
        {
            if(GameController.MoneyPerSecond > 0)
                StartEvents();
            yield return new WaitForSeconds(1);
        }
    }

    void StartEvents()
    {
        foreach (var bonus in Events)
        {
            if (bonus.IsCurrentlyActive)
                continue;

            var RNG = Random.Range(0f, 100f);
            if (RNG <= bonus.Chance)
            {
                bonus.StartEvent();
            }
        }
    }

    void DetectTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            var touchPosition = Input.GetTouch(0).position;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out hit))
            {
                var eventComponent = hit.collider.GetComponent<BonusEvent>();
                if (eventComponent != null && eventComponent.IsCurrentlyActive)
                {
                    var reward = GameController.MoneyPerSecond * eventComponent.RewardMultiplier;
                    GameController.AddMoney(reward);
                    eventComponent.EndEvent();

                    var newX = Mathf.Clamp(touchPosition.x, MinPosition.x, MaxPosition.x);
                    var newPosition = new Vector3(newX, touchPosition.y);
                    var rewardText = Instantiate(RewardTextPrefab, newPosition, Quaternion.identity, RewardTextParent);
                    rewardText.RewardText.text = "+" + Utils.MoneyToString(reward);
                }
            }
        }
    }
}
