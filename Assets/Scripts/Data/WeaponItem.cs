using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item
{
    public WeaponItem()
    {
        ItemType = ItemType.WEAPON;
        EquipableType = EquipableType.WEAPON;
    }

    public WeaponType WeaponType;
    public AmmoType AmmoType;
    public AmmoType AmmoType2;
    public string ObjectTemplateName;

    public int WeaponDamage;
    public int AmmoLeft;

}
