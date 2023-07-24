using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerInventory : Inventory
{
    // Start is called before the first frame update
    void Start()
    {
        Items.Add(ItemRegistry.DarkMatter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
