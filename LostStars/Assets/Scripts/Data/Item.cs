using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public Item()
    {
        this.ItemType = ItemType.BASIC;
        this.EquipableType = EquipableType.NONE;
    }

    public ItemType ItemType { get; protected set; }

    public EquipableType EquipableType { get; set; }

    public string Name;
    public string Id;
    public AmmoType Ammo;
}
