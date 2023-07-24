using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellType
{
    public string TypeName;

    public static SpellType STORM = new SpellType { TypeName = "Storm" };
    public static SpellType SHADOW = new SpellType { TypeName = "Shadow" };
    public static SpellType BLOOD = new SpellType { TypeName = "Blood" };
    public static SpellType DREAM = new SpellType { TypeName = "Dream" };
    public static SpellType BONE = new SpellType { TypeName = "Bone" };
}