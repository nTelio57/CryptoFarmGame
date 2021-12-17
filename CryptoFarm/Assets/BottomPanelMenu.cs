using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomPanelMenu : MonoBehaviour
{
    public Animator CameraAnimator;
    public GameObject[] Tabs;
    private Animator[] Animators;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Animators = new Animator[Tabs.Length];
        for (int i = 0; i < Tabs.Length; i++)
        {
            Animators[i] = Tabs[i].GetComponent<Animator>();
        }
    }

    public void OnButtonClick(int tabId)
    {
        Tabs[tabId].SetActive(true);
        Animators[tabId].SetBool("IsActive", true);
        CameraAnimator.SetBool("IsRaised", true);
    }

    public void OnExitClick(int tabId)
    {
        Animators[tabId].SetBool("IsActive", false);
        CameraAnimator.SetBool("IsRaised", false);
    }
}
