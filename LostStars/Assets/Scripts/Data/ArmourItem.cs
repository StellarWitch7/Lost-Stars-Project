using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourItem : Item
{
    public ArmourItem()
    {
        this.ItemType = ItemType.ARMOUR;
    }
    
    public int ArmourResistance;

    public ArmourType ArmourType;
}