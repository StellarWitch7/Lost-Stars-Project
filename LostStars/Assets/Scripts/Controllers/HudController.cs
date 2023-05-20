using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public TextMeshProUGUI BottomLabel;
    public TextMeshProUGUI AmmoAmountLabel;
    public TextMeshProUGUI WeaponNameLabel;
    public TextMeshProUGUI HealthAmountLabel;
    public CrosshairScript Crosshair;
    public BarScript HealthBar;
    public BarScript EnergyBar;
    public GameObject ReloadingGraphic;
    private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBottomLabel(string text)
    {
        if (BottomLabel != null)
        {
            BottomLabel.SetText(text);
        }
    }

    public void SetPlayer(GameObject player)
    {
        _player = player;
    }

    public void SetAmmoAmountLabel(int ammoLeft, int ammoCap)
    {

        AmmoAmountLabel.SetText(ammoLeft + "/" + ammoCap);
    }

    public void UpdateHealth(float healthCurrent, float healthMax)
    {
        SetHealthAmountLabel(healthCurrent, healthMax);
        SetHealthBar(healthCurrent, healthMax);
    }

    public void SetHealthAmountLabel(float healthCurrent, float healthMax)
    {
        HealthAmountLabel.SetText(healthCurrent + "/" + healthMax);
    }

    public void SetHealthBar(float healthCurrent, float healthMax)
    {
        HealthBar.Set(healthCurrent, healthMax);
    }

    public void ClearAmmoLabel()
    {
        AmmoAmountLabel.SetText("");
    }

    public void UpdateEnergy(float energyCurrent, float energyMax)
    {
        SetEnergyBar(energyCurrent, energyMax);
    }

    public void SetEnergyBar(float energyCurrent, float energyMax)
    {
        EnergyBar.Set(energyCurrent, energyMax);
    }

    public void SetReloadingGraphic(bool b)
    {
        ReloadingGraphic.SetActive(b);
    }
}
