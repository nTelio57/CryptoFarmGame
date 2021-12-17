using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject MiningPanel;
    public GameObject ItemPanel;
    public GameObject EnergyPanel;
    public GameObject RentingPanel;

    public void HideAllPanels()
    {
        MiningPanel.SetActive(false);
        //ItemPanel.SetActive(false);
       // EnergyPanel.SetActive(false);
       // RentingPanel.SetActive(false);
    }

    public void OnMiningClick()
    {
        MiningPanel.SetActive(true);
    }
}

