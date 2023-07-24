using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoType
{
    public string TypeName;

    public static AmmoType CELL = new AmmoType { TypeName = "Power Cell" };
    public static AmmoType BALLISTIC_5MM = new AmmoType { TypeName = "5mm" };
    public static AmmoType BALLISTIC_9MM = new AmmoType { TypeName = "9mm" };
    public static AmmoType BALLISTIC_13MM = new AmmoType { TypeName = "13mm" };
    public static AmmoType BALLISTIC_17MM = new AmmoType { TypeName = "17mm" };
    public static AmmoType BALLISTIC_21MM = new AmmoType { TypeName = "21mm" };
    public static AmmoType BALLISTIC_SHELL = new AmmoType { TypeName = "Shell" };
}
