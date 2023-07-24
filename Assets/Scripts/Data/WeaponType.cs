using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponType
{
    public string TypeName;

    public static WeaponType LASER = new WeaponType { TypeName = "Laser" };
    public static WeaponType PROJECTILE = new WeaponType { TypeName = "Projectile" };
    public static WeaponType MELEE = new WeaponType { TypeName = "Melee" };
    public static WeaponType BALLISTIC = new WeaponType { TypeName = "Ballistic" };
}