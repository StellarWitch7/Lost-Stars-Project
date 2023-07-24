using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicItem : Item
{
    public RelicItem()
    {
        this.ItemType = ItemType.RELIC;
    }

    public RelicType RelicType;

    public int RelicPower;
}