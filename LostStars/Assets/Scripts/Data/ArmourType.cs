using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourType
{
    public string TypeName;

    public static ArmourType ELECTRIC = new ArmourType { TypeName = "Electric" };
    public static ArmourType PHYSICAL = new ArmourType { TypeName = "Physical" };
}
