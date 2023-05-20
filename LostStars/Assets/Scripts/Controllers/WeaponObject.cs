using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    public WeaponItem WeaponItem { get; set; }
    public float Recoil { get; set; }
    public float Spread { get; set; }
    public float SpreadFactor { get; set; }
    public float MinSpread { get; set; }
    public float MaxSpread { get; set; } //keep below 10
    public float ReloadTime { get; set; }
    public float Range { get; set; }
    public float EffectiveRange { get; set; }
    public float Damage { get; set; }
    public float TimeBetweenShots { get; set; }
    public int AmmoCostPerShot { get; set; }
    public int BulletsPerShot { get; set; }
    public int AmmoCapacity { get; set; }
    public int AmmoLeft { get; set; }
    public bool Automatic { get; set; }
    public bool Reloading { get; set; }
    public WeaponType WeaponType { get; set; }
    public AmmoType AmmoType { get; set; }
    public AmmoType AmmoType2 { get; set; }

    private FirstPersonController _fpc;
    private PlayerInventory _inv;
    private GameObject _player;
    private bool _triggerHeld;
    private float _timeSinceLastShot;
    private bool _scoped;

    private void Start()
    {
        
    }

    private void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
        Spread = Spread - (Time.deltaTime * SpreadFactor) < MinSpread ? MinSpread : Spread - (Time.deltaTime * SpreadFactor);
        Spread = Spread < MinSpread ? MinSpread : Spread;
        Spread = Spread > MaxSpread ? MaxSpread : Spread;
        //The field _player is being set to null at some point
        _fpc.SetUiSpread(Spread, MinSpread, MaxSpread);

        if (Reloading) //use this to show a graphic telling the player they are reloading
        {
            _fpc.SetReloadingGraphic(true);
        }
        else
        {
            _fpc.SetReloadingGraphic(false);
        }
    }

    public void StartReloading()
    {
        if (AmmoLeft < AmmoCapacity && Reloading == false)
        {
            bool b = AmmoType2 != null ? true : false;
            StartCoroutine(Reload(b));
        }
    }

    IEnumerator Reload(bool useSecondaryAmmo) //Reloading is still broken
    {
        SetReloading(true);
        RefreshAmmoUi();

        int possibleAmmoToAdd = AmmoCapacity - AmmoLeft;
        int ammoToAdd = 0;
        var deletedItems = new List<Item>();

        for (var i = possibleAmmoToAdd; i > 0; i--)
        {
            var itemsToDelete = new List<Item>();

            foreach (var item in GetInv().Items)
            {
                if (item.Ammo == AmmoType)
                {
                    if (!useSecondaryAmmo)
                    {
                        itemsToDelete.Add(item);
                        ammoToAdd++;
                        break;
                    }
                    else
                    {
                        foreach (var item2 in GetInv().Items)
                        {
                            if (item2.Ammo == AmmoType2)
                            {
                                itemsToDelete.Add(item);
                                itemsToDelete.Add(item2);
                                ammoToAdd++;
                                break;
                            }
                        }

                        break;
                    }
                }
            }

            foreach (var item in itemsToDelete)
            {
                GetInv().Items.Remove(item);
                deletedItems.Add(item);
            }
        }

        if (ammoToAdd > 0)
        {
            yield return new WaitForSeconds(ReloadTime);
        }
        else
        {
            foreach (var item in deletedItems)
            {
                GetInv().Items.Add(item);
            }
        }

        AmmoLeft += ammoToAdd;

        SetReloading(false);
        RefreshAmmoUi();
        RefreshWeaponItem();
    }

    private void RefreshWeaponItem()
    {
        _fpc.WeaponHolder.EquippedWeapon.AmmoLeft = AmmoLeft;
    }

    public virtual void SetReloading(bool reloading)
    {
        Reloading = reloading;
    }

    public virtual PlayerInventory GetInv()
    {
        return _inv;
    }

    public virtual void SetAmmoLeft(int ammoLeft)
    {
        AmmoLeft = ammoLeft;
    }

    public void DeleteSelf()
    {

    }

    public virtual void PrimaryFire(Vector3 cameraPos, Vector3 fwd)
    {
        if (Reloading == false)
        {
            bool shoot = false;

            if (Automatic)
            {
                if (_timeSinceLastShot >= TimeBetweenShots)
                {
                    shoot = true;
                    _timeSinceLastShot = 0;
                }
            }
            else
            {
                if (!_triggerHeld && _timeSinceLastShot >= TimeBetweenShots)
                {
                    shoot = true;
                    _triggerHeld = true;
                    _timeSinceLastShot = 0;
                }
            }

            if (shoot == true)
            {
                var shotsToFire = BulletsPerShot;

                if (BulletsPerShot > AmmoLeft)
                {
                    shotsToFire = AmmoLeft;
                }

                if (shotsToFire > 0)
                {
                    for (var i = shotsToFire; i > 0; i--)
                    {
                        float spreadReducer = 100;
                        var trajectory = new Vector3(Random.Range(fwd.x - Spread / spreadReducer, fwd.x + Spread / spreadReducer),
                            Random.Range(fwd.y - Spread / spreadReducer, fwd.y + Spread / spreadReducer),
                            Random.Range(fwd.z - Spread / spreadReducer, fwd.z + Spread / spreadReducer));

                        if (_scoped)
                        {
                            trajectory = fwd;
                        }

                        if (Physics.Raycast(cameraPos, trajectory, out RaycastHit hitInfo, Range))
                        {
                            GameObject target = hitInfo.transform.gameObject;

                            if (target.GetComponent<NonPlayerCharacter>() is NonPlayerCharacter character)
                            {
                                character.Damage(Damage);

                                Debug.DrawLine(cameraPos, cameraPos + trajectory * Range, Color.green, 3f);
                            }
                            else if (target.GetComponentInParent<NonPlayerCharacter>() is NonPlayerCharacter characterParent)
                            {
                                characterParent.Damage(Damage);

                                Debug.DrawLine(cameraPos, cameraPos + trajectory * Range, Color.green, 3f);
                            }
                            else
                            {
                                Debug.DrawLine(cameraPos, cameraPos + trajectory * Range, Color.red, 3f);
                            }
                        }
                    }

                    AmmoLeft -= shotsToFire;
                    Spread += MaxSpread / 2;

                    ApplyRecoil(Recoil);
                    PrimaryFireGraphics();
                    RefreshAmmoUi();
                    RefreshWeaponItem();
                }
            }
        }
    }

    public void ApplyRecoil(float amount)
    {
        _fpc.ApplyRecoil(amount);
    }

    public void RefreshAmmoUi()
    {
        _fpc.RefreshAmmoUi(AmmoLeft, AmmoCapacity);
    }

    public virtual void PrimaryFireGraphics()
    {

    }

    public virtual void SecondaryFire(Vector3 cameraPos, Vector3 fwd)
    {

    }

    public void SetTriggerHeld(bool trigger)
    {
        _triggerHeld = trigger;
    }

    public void SetInv(PlayerInventory inv)
    {
        _inv = inv;
    }

    public void SetPlayer(GameObject player)
    {
        _player = player;
        _fpc = _player.GetComponent<FirstPersonController>();
    }
}
