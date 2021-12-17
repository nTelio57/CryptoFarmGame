using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class LocationController : MonoBehaviour
{
    [SerializeField]
    private GameController GameController;

    public Location[] Locations;
    public HorizontalScrollSnap LocationScroll;
    public int CurrentLocationId;

    [Header("UI")]
    public Text CoinNameText;
    public Text CoinRatioText;
    public Text LocationNameText;
    public Text LocationPriceText;
    public Text BehindButtonText;
    public Button LocationBuyButton;

    private Location _selectedLocation;
    private int _selectedLocationId;

    private void Start()
    {
        _selectedLocation = Locations[CurrentLocationId];
        _selectedLocationId = CurrentLocationId;
        SetLocation();
        GameController.SetLocationRatio(Locations[CurrentLocationId].CoinRatio);
    }

    private void Update()
    {
        IsSelectedLocationBuyable();
    }

    void IsSelectedLocationBuyable()
    {
        if (_selectedLocationId > CurrentLocationId && GameController.Money < _selectedLocation.LocationPrice)//no money
        {
            LocationBuyButton.gameObject.SetActive(true);
            LocationBuyButton.interactable = false;
        }
        else if (_selectedLocationId > CurrentLocationId && GameController.Money >= _selectedLocation.LocationPrice)
        {
            LocationBuyButton.gameObject.SetActive(true);
            LocationBuyButton.interactable = true;
        }
    }

    public void SetLocation()
    {
        _selectedLocation = Locations[LocationScroll.CurrentPage];
        _selectedLocationId = LocationScroll.CurrentPage;

        CoinNameText.text = _selectedLocation.CoinName;
        CoinRatioText.text = $"1 coin = {_selectedLocation.CoinRatio}$";
        LocationNameText.text = _selectedLocation.LocationName;
        LocationPriceText.text = Utils.MoneyToString(_selectedLocation.LocationPrice, true);

        //Button
        if (_selectedLocationId < CurrentLocationId) //previous location
        {
            BehindButtonText.text = "PREVIOUS LOCATION";
            LocationBuyButton.gameObject.SetActive(false);
        }
        else if (_selectedLocationId == CurrentLocationId) //Current location
        {
            BehindButtonText.text = "YOU ARE HERE";
            LocationBuyButton.gameObject.SetActive(false);
        }
        else if (_selectedLocationId > CurrentLocationId && GameController.Money < _selectedLocation.LocationPrice)//no money
        {
            LocationBuyButton.gameObject.SetActive(true);
            LocationBuyButton.interactable = false;
        }
        else
        {
            LocationBuyButton.gameObject.SetActive(true);
            LocationBuyButton.interactable = true;
        }
    }

    public void OnBuyClick()
    {
        if (GameController.Money >= _selectedLocation.LocationPrice)
        {
            SetLocation(_selectedLocationId);

            GameController.Reset();

            gameObject.SetActive(false);

            GameController.SaveData();
        }
    }

    public void SetLocation(int id)
    {
        Locations[CurrentLocationId].Model.SetActive(false);
        CurrentLocationId = id;
        Locations[CurrentLocationId].Model.SetActive(true);
        GameController.SetCameraPosition(Locations[CurrentLocationId].CameraTransform.position);
        GameController.SetLocationRatio(Locations[CurrentLocationId].CoinRatio);
    }

    public double GetCurrentLocationRatio()
    {
        return Locations[CurrentLocationId].CoinRatio;
    }

    public int GetCurrentLocationId()
    {
        return CurrentLocationId;
    }

    public void SetAvailableLocationPage()
    {
        LocationScroll.GoToScreen(CurrentLocationId, true);
    }
}
