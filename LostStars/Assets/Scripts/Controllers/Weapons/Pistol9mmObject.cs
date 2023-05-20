using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol9mmObject : WeaponObject
{
    public Pistol9mmObject()
    {
        WeaponItem = ItemRegistry.Pistol9mm;
        Recoil = 2;
        SpreadFactor = 10;
        MinSpread = 1;
        MaxSpread = 5;
        Range = 100;
        EffectiveRange = 50;
        TimeBetweenShots = 0.08f;
        Damage = ItemRegistry.Pistol9mm.WeaponDamage;
        ReloadTime = 2;
        AmmoCostPerShot = 1;
        BulletsPerShot = 1;
        AmmoCapacity = 15;
        Automatic = false;
        WeaponType = ItemRegistry.Pistol9mm.WeaponType;
        AmmoType = ItemRegistry.Pistol9mm.AmmoType;
    }

    public override void PrimaryFire(Vector3 cameraPos, Vector3 fwd)
    {
        base.PrimaryFire(cameraPos, fwd);
    }

    public override void PrimaryFireGraphics()
    {

    }

    public override void SecondaryFire(Vector3 cameraPos, Vector3 fwd)
    {

    }
}
