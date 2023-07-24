using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TritaneskObject : WeaponObject
{
    public TritaneskObject()
    {
        WeaponItem = ItemRegistry.Tritanesk;
        Recoil = 0;
        SpreadFactor = 10;
        MinSpread = 1.5f;
        MaxSpread = 4;
        Range = 200;
        EffectiveRange = 150;
        TimeBetweenShots = 0.5f;
        Damage = ItemRegistry.Tritanesk.WeaponDamage;
        ReloadTime = 7;
        AmmoCostPerShot = 1;
        BulletsPerShot = 1;
        AmmoCapacity = 1;
        Automatic = false;
        WeaponType = ItemRegistry.Tritanesk.WeaponType;
        AmmoType = ItemRegistry.Tritanesk.AmmoType;
        AmmoType2 = ItemRegistry.Tritanesk.AmmoType2;
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
