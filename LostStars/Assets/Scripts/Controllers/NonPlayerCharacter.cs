using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NonPlayerCharacter : MonoBehaviour
{
    public float HealthCurrent;
    public float HealthMax;
    public float HealDelay;
    public float TimeBetweenNaturaHeals;
    public bool HealingAllowed;
    public float NaturalHealAmount;
    public BarScript HealthBar;
    public TextMeshProUGUI HealthAmountLabel;
    public GameObject Canvas;

    private float _timeUntilHeal;
    private GameObject _camera;

    // Start is called before the first frame update
    void Start()
    {
        Heal(HealthMax);
        _timeUntilHeal = HealDelay;

        OverrideStart();
    }

    public virtual void OverrideStart()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_camera != null)
        {
            StatsBillboard();
        }
        else
        {
            _camera = GameObject.Find("Main Camera");
        }

        if (HealingAllowed && _timeUntilHeal <= 0)
        {
            Heal(NaturalHealAmount);
            _timeUntilHeal = TimeBetweenNaturaHeals;
        }

        _timeUntilHeal -= Time.deltaTime;

        if (HealthCurrent > HealthMax)
        {
            HealthCurrent = HealthMax;
            UpdateHealth(HealthCurrent, HealthMax);
        }

        OverrideUpdate();
    }

    public virtual void OverrideUpdate()
    {

    }

    public virtual void Damage(float amount)
    {
        HealthCurrent -= amount;
        _timeUntilHeal = HealDelay;
        UpdateHealth(HealthCurrent, HealthMax);

        if (HealthCurrent <= 0)
        {
            SelfDestruct();
        }
    }

    public virtual void Heal(float amount)
    {
        if (HealthCurrent < HealthMax)
        {
            HealthCurrent += amount;
        }

        UpdateHealth(HealthCurrent, HealthMax);
    }

    public virtual void SelfDestruct()
    {
        GameObject.DestroyImmediate(this.gameObject);
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

    public void StatsBillboard()
    {
        Canvas.transform.rotation = Quaternion.LookRotation(Canvas.transform.position - _camera.transform.position);
    }
}
