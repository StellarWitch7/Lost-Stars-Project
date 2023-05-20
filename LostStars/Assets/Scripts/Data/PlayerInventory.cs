using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    public float Credits;
    public float WitchTokens;
    public float Experience;
    public int Level;
    public WeaponItem PrimaryWeapon;
    public WeaponItem SecondaryWeapon;
    public ArmourItem ChestArmour;
    public ArmourItem LegArmour;
    public ArmourItem HeadArmour;
    public ArmourItem FootArmour;
    public RelicItem PrimaryRelic;
    public RelicItem SecondaryRelic;
    public RelicItem TertiaryRelic;
    public bool EquippedIsPrimary;
}
