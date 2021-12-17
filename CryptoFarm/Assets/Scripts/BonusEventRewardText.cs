using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusEventRewardText : MonoBehaviour
{
    public RectTransform rectTransform;
    public Text RewardText;
    public float Speed;
    public float DestructionTimer;
    private Vector3 newPosition;

    private void Start()
    {
        newPosition = rectTransform.anchoredPosition;
        Destroy(gameObject, DestructionTimer);
    }

    private void Update()
    {
        newPosition.y += Speed * Time.deltaTime;
        rectTransform.anchoredPosition = newPosition;
    }
}
