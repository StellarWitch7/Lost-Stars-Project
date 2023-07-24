using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemRegistry
{
    private static readonly ItemList<Item> _registeredItems = new ItemList<Item>();
    private static readonly ItemList<WeaponItem> _registeredWeapons = new ItemList<WeaponItem>();
    private static readonly ItemList<ArmourItem> _registeredArmours = new ItemList<ArmourItem>();
    private static readonly ItemList<RelicItem> _registeredRelics = new ItemList<RelicItem>();

    public static ItemList<Item> Items => _registeredItems;

    public static ItemList<WeaponItem> Weapons => _registeredWeapons;

    public static ItemList<ArmourItem> Armours => _registeredArmours;

    public static ItemList<RelicItem> Relics => _registeredRelics;

    //Methods
    public static Item Register(Item item)
    {
        _registeredItems.Add(item);
        return item;
    }

    public static WeaponItem Register(WeaponItem item)
    {
        _registeredWeapons.Add(item);
        return item;
    }

    public static ArmourItem Register(ArmourItem item)
    {
        _registeredArmours.Add(item);
        return item;
    }

    public static RelicItem Register(RelicItem item)
    {
        _registeredRelics.Add(item);
        return item;
    }

    public static WeaponItem ItemToWeapon(Item data)
    {
        var weaponDef = Weapons.GetById(data.Id);
        var newWeapon = new WeaponItem();
        newWeapon.Id = data.Id;
        newWeapon.Name = data.Name;
        newWeapon.WeaponDamage = weaponDef.WeaponDamage;
        newWeapon.WeaponType = weaponDef.WeaponType;
        newWeapon.ObjectTemplateName = weaponDef.ObjectTemplateName;
        newWeapon.AmmoType = weaponDef.AmmoType;
        newWeapon.AmmoLeft = weaponDef.AmmoLeft; //this doesn't work. The ammo inside the weapon will always be lost. 

        return newWeapon;
    }
    public static ArmourItem ItemToArmour(Item data)
    {
        var armourDef = Armours.GetById(data.Id);
        var newArmour = new ArmourItem();
        newArmour.Id = data.Id;
        newArmour.Name = data.Name;
        newArmour.ArmourResistance = armourDef.ArmourResistance;
        newArmour.ArmourType = armourDef.ArmourType;
        newArmour.EquipableType = armourDef.EquipableType;

        return newArmour;
    }

    public static RelicItem ItemToRelic(Item data)
    {
        var relicDef = Relics.GetById(data.Id);
        var newRelic = new RelicItem();
        newRelic.Id = data.Id;
        newRelic.Name = data.Name;
        newRelic.RelicPower = relicDef.RelicPower;
        newRelic.RelicType = relicDef.RelicType;

        return newRelic;
    }

    //Items
    public static readonly Item Crystal = Register(new Item { Id = "crystal", Name = "Crystal" });
    public static readonly Item DarkMatter = Register(new Item { Id = "dark_matter", Name = "Dark Matter" });
    public static readonly Item PowerCell = Register(new Item { Id = "power_cell", Name = "Power Cell", Ammo = AmmoType.CELL });
    public static readonly Item Ammo5mm = Register(new Item { Id = "5mm_ammo", Name = "5mm Ammo", Ammo = AmmoType.BALLISTIC_5MM });
    public static readonly Item Ammo9mm = Register(new Item { Id = "9mm_ammo", Name = "9mm Ammo", Ammo = AmmoType.BALLISTIC_9MM });
    public static readonly Item Ammo13mm = Register(new Item { Id = "13mm_ammo", Name = "13mm Ammo", Ammo = AmmoType.BALLISTIC_13MM });
    public static readonly Item Ammo17mm = Register(new Item { Id = "17mm_ammo", Name = "17mm Ammo", Ammo = AmmoType.BALLISTIC_17MM });
    public static readonly Item Ammo21mm = Register(new Item { Id = "21mm_ammo", Name = "21mm Ammo", Ammo = AmmoType.BALLISTIC_21MM });
    public static readonly Item AmmoShell = Register(new Item { Id = "shell_ammo", Name = "Shell Ammo", Ammo = AmmoType.BALLISTIC_SHELL });

    //Weapons
    public static readonly WeaponItem LaserPistol = Register(new WeaponItem
    {
        Id = "laser_pistol",
        Name = "Laser Pistol",
        ObjectTemplateName = "LaserPistolObject",
        WeaponType = WeaponType.LASER,
        AmmoType = AmmoType.CELL,
        WeaponDamage = 13
    });
    public static readonly WeaponItem Pistol9mm = Register(new WeaponItem
    {
        Id = "9mm_pistol",
        Name = "9mm Pistol",
        ObjectTemplateName = "Pistol9mmObject",
        WeaponType = WeaponType.BALLISTIC,
        AmmoType = AmmoType.BALLISTIC_9MM,
        WeaponDamage = 14
    });
    public static readonly WeaponItem Tritanesk = Register(new WeaponItem
    {
        Id = "tritanesk",
        Name = "Tritanesk Hand-Held Gravity Cannon",
        ObjectTemplateName = "TritaneskObject",
        WeaponType = WeaponType.BALLISTIC,
        AmmoType = AmmoType.BALLISTIC_21MM,
        AmmoType2 = AmmoType.CELL,
        WeaponDamage = 179
    });
    public static readonly WeaponItem AssaultRifle13mm = Register(new WeaponItem
    {
        Id = "assault_rifle_13mm",
        Name = "13mm Assault Rifle",
        ObjectTemplateName = "AssaultRifle13mmObject",
        WeaponType = WeaponType.BALLISTIC,
        AmmoType = AmmoType.BALLISTIC_13MM,
        WeaponDamage = 21
    });

    //Armour
    public static readonly ArmourItem ChestShield = Register(new ArmourItem
    {
        Id = "chest_shield",
        Name = "Chest Shielding",
        ArmourType = ArmourType.ELECTRIC,
        EquipableType = EquipableType.CHEST,
        ArmourResistance = 20
    });

    //Relics
    public static readonly RelicItem StormEnhancer = Register(new RelicItem
    {
        Id = "storm_enhancer",
        Name = "Storm Enhancer",
        RelicType = RelicType.STORM,
        RelicPower = 6
    });

    public static readonly RelicItem JumpEnhancer = Register(new RelicItem
    {
        Id = "jump_enhancer",
        Name = "Jump Enhancer",
        RelicType = RelicType.STORM,
        RelicPower = 1
    });
}