using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpellRegistry
{
    private static List<Spell> _registeredSpells = new List<Spell>();

    //Methods
    public static Spell Register(Spell spell)
    {
        _registeredSpells.Add(spell);
        return spell;
    }

    //Spells
    public static Spell FIREBALL = Register(new Spell { Id = "fireball", Name = "Fireball", Type = SpellType.STORM, Damage = 25 });
}