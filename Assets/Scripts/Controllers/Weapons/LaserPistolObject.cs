using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPistolObject : WeaponObject
{
    public LaserPistolObject()
    {
        WeaponItem = ItemRegistry.LaserPistol;
        Recoil = 0;
        SpreadFactor = 10;
        MinSpread = 0;
        MaxSpread = 1;
        Range = 120;
        EffectiveRange = 100;
        TimeBetweenShots = 0.05f;
        Damage = ItemRegistry.LaserPistol.WeaponDamage;
        ReloadTime = 3;
        AmmoCostPerShot = 1;
        BulletsPerShot = 1;
        AmmoCapacity = 14;
        Automatic = false;
        WeaponType = ItemRegistry.LaserPistol.WeaponType;
        AmmoType = ItemRegistry.LaserPistol.AmmoType;
    }

    public override void PrimaryFire(Vector3 cameraPos, Vector3 fwd)
    {
        base.PrimaryFire(cameraPos, fwd);
    }

    public override void SecondaryFire(Vector3 cameraPos, Vector3 fwd)
    {

    }
}
