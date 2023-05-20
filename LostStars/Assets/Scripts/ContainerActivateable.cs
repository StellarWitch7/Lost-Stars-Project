using StarterAssets;
using UnityEngine;

public class ContainerActivateable : Activateable
{
    private string _activateText;
    public ActivateEvent Opened;
    public ContainerInventory Inventory = new ContainerInventory();

    public override string ActionText => _activateText;

    public void Start()
    {
        _activateText = "Press E to open";

        //For testing purposes
        Inventory.Items.Add(ItemRegistry.Pistol9mm);
        for (var i = 50; i > 0; i--)
        {
            Inventory.Items.Add(ItemRegistry.Ammo9mm);
        }
        Inventory.Items.Add(ItemRegistry.LaserPistol);
        for (var i = 50; i > 0; i--)
        {
            Inventory.Items.Add(ItemRegistry.PowerCell);
        }
        Inventory.Items.Add(ItemRegistry.Tritanesk);
        for (var i = 50; i > 0; i--)
        {
            Inventory.Items.Add(ItemRegistry.Ammo21mm);
        }
        Inventory.Items.Add(ItemRegistry.AssaultRifle13mm);
        for (var i = 50; i > 0; i--)
        {
            Inventory.Items.Add(ItemRegistry.Ammo13mm);
        }
    }

    public override void Activate(GameObject user)
    {
        this.Opened?.Invoke(user);
    }

    public void OpenInventory(GameObject user)
    {
        user.GetComponent<FirstPersonController>().OpenInventory(this.gameObject);
    }
}
