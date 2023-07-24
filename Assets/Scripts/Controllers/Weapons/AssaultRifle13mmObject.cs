using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle13mmObject : WeaponObject
{
    public AssaultRifle13mmObject()
    {
        WeaponItem = ItemRegistry.AssaultRifle13mm;
        Recoil = 1.5f;
        SpreadFactor = 10;
        MinSpread = 1;
        MaxSpread = 5.5f;
        Range = 130;
        EffectiveRange = 70;
        TimeBetweenShots = 0.1575f;
        Damage = ItemRegistry.AssaultRifle13mm.WeaponDamage;
        ReloadTime = 3.75f;
        AmmoCostPerShot = 1;
        BulletsPerShot = 1;
        AmmoCapacity = 35;
        Automatic = true;
        WeaponType = ItemRegistry.AssaultRifle13mm.WeaponType;
        AmmoType = ItemRegistry.AssaultRifle13mm.AmmoType;
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