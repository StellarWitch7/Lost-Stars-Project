using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ItemList<T> : List<T>
    where T : Item
{
    public T GetById(string id)
    {
        foreach (var item in this)
        {
            if (string.Equals(item.Id, id, StringComparison.OrdinalIgnoreCase))
            {
                return item;
            }
        }

        return null;
    }

    public T GetByAmmo(AmmoType type)
    {
        foreach (var item in this)
        {
            if (type == item.Ammo)
            {
                return item;
            }
        }

        return null;
    }
}
