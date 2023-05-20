using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemType
{
    public string TypeName;

    public static ItemType WEAPON = new ItemType { TypeName = "Weapon" };
    public static ItemType ARMOUR = new ItemType { TypeName = "Armour" };
    public static ItemType BASIC = new ItemType { TypeName = "Basic" };
    public static ItemType RELIC = new ItemType { TypeName = "Relic" };
}
