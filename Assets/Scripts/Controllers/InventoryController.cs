using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public List<ItemStack> PlayerStacks = new List<ItemStack>();
    public List<ItemStack> TargetStacks = new List<ItemStack>();
    public List<ItemStack> SelectedStacks = new List<ItemStack>();
    public List<ItemStack> KillStacks = new List<ItemStack>();
    public List<InventoryItemUi> PlayerItemUis = new List<InventoryItemUi>();
    public List<InventoryItemUi> TargetItemUis = new List<InventoryItemUi>();
    public GameObject PlayerContainer;
    public GameObject TargetContainer;
    public GameObject PrimaryWeaponSlot;
    public GameObject SecondaryWeaponSlot;
    public GameObject HeadArmourSlot;
    public GameObject ChestArmourSlot;
    public GameObject LegArmourSlot;
    public GameObject FootArmourSlot;
    public GameObject PrimaryRelicSlot;
    public GameObject SecondaryRelicSlot;
    public GameObject TertiaryRelicSlot;
    private GameObject _storageTarget;
    public InventoryItemUi ItemTemplate;
    public EquipmentSlotUi WeaponItemTemplate;
    public EquipmentSlotUi ArmourItemTemplate;
    public EquipmentSlotUi RelicItemTemplate;
    public PlayerInventory PlayerInventory = new PlayerInventory();

    // Start is called before the first frame update
    void Start()
    {
        PlayerInventory.Items.Add(ItemRegistry.ChestShield);
        PlayerInventory.Items.Add(ItemRegistry.StormEnhancer);
        PlayerInventory.Items.Add(ItemRegistry.JumpEnhancer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GeneratePlayerItems()
    {
        PlayerStacks.Clear();

        foreach (var item in PlayerInventory.Items)
        {
            var existing = this.PlayerStacks.FirstOrDefault(n => n.ItemData == item);

            if (existing != null)
            {
                existing.Count++;
            }
            else
            {
                var newStack = new ItemStack { ItemData = item, Count = 1, StoredIn = PlayerInventory };
                PlayerStacks.Add(newStack);
            }
        }
    }

    public void GenerateTargetItems(GameObject target)
    {
        var invTarget = target.GetComponent<ContainerActivateable>().Inventory;

        TargetStacks.Clear();

        foreach (var item in invTarget.Items)
        {
            var existing = this.TargetStacks.FirstOrDefault(n => n.ItemData == item);

            if (existing != null)
            {
                existing.Count += 1;
            }
            else
            {
                var newStack = new ItemStack { ItemData = item, Count = 1, StoredIn = invTarget };
                TargetStacks.Add(newStack);
            }
        }
    }

    public void SetStorageTarget(GameObject target)
    {
        _storageTarget = target;
    }

    public void PopulatePlayerInv()
    {
        for (int i = 0; i < this.PlayerContainer.transform.childCount; i++)
        {
            var child = this.PlayerContainer.transform.GetChild(i);
            var itemUi = child.GetComponent<InventoryItemUi>();
            itemUi.DeleteSelf();
            i--;
        }

        foreach (var stack in PlayerStacks)
        {
            AddInventoryItemUi(stack, this.PlayerContainer, PlayerInventory);
        }

        PlayerStacks.Clear();
        PlayerItemUis.Clear();
    }

    public void PopulateTargetInv(Inventory targetInv)
    {
        for (int i = 0; i < this.TargetContainer.transform.childCount; i++)
        {
            var child = this.TargetContainer.transform.GetChild(i);
            var itemUi = child.GetComponent<InventoryItemUi>();
            itemUi.DeleteSelf();
            i--;
        }

        foreach (var stack in TargetStacks)
        {
            AddInventoryItemUi(stack, this.TargetContainer, targetInv);
        }

        TargetStacks.Clear();
        TargetItemUis.Clear();
    }

    public void AddInventoryItemUi(ItemStack stack, GameObject target, Inventory targetInv)
    {
        var newItemUi = Instantiate(this.ItemTemplate);
        newItemUi.transform.SetParent(target.transform, false);
        newItemUi.DisplayName = stack.ItemData.Name;
        newItemUi.ItemId = stack.ItemData.Id;
        newItemUi.Amount = stack.Count;
        newItemUi.ItemStack = stack;
        newItemUi.CurrentInventory = targetInv;
        newItemUi.SetController(this);
        newItemUi.SetText();

        if (target == PlayerContainer)
        {
            PlayerItemUis.Add(newItemUi);
        }
        else if (target == TargetContainer)
        {
            TargetItemUis.Add(newItemUi);
        }
    }

    public void UpdateStackSelection(ItemStack stack, bool isSelected)
    {
        stack.IsSelected = isSelected;

        if (isSelected)
        {
            this.SelectedStacks.Add(stack);
        }
        else
        {
            this.SelectedStacks.Remove(stack);
        }
    }

    public void AttemptMove(GameObject uiTarget)
    {
        if(_storageTarget != null)
        {
            MoveStack(_storageTarget, uiTarget);
        }
    }

    public void MoveStack(GameObject storageTarget, GameObject uiTarget)
    {
        var invTarget = storageTarget.GetComponentInParent<ContainerActivateable>().Inventory;

        foreach (var stack in SelectedStacks)
        {
            var item = stack.ItemData;

            if (uiTarget == PlayerContainer)
            {
                while (stack.Count > 0)
                {
                    stack.StoredIn.Items.Remove(item);
                    PlayerInventory.Items.Add(item);
                    stack.Count--;
                }
            }
            else
            {
                while (stack.Count > 0)
                {
                    stack.StoredIn.Items.Remove(item);
                    invTarget.Items.Add(item);
                    stack.Count--;
                }
            }

            stack.IsSelected = false;
        }

        SelectedStacks.Clear();
        GeneratePlayerItems();
        GenerateTargetItems(storageTarget);
        PopulatePlayerInv();
        PopulateTargetInv(invTarget);
    }

    public void GenerateEquipped()
    {
        ClearEquippedUis();

        if (PrimaryWeaponSlot.gameObject.transform.childCount != 1 && PlayerInventory.PrimaryWeapon != null)
        {
            AddSlotWeaponUi(PlayerInventory.PrimaryWeapon, PrimaryWeaponSlot, SlotType.PRIMARY_WEAPON);
        }

        if (SecondaryWeaponSlot.gameObject.transform.childCount != 1 && PlayerInventory.SecondaryWeapon != null)
        {
            AddSlotWeaponUi(PlayerInventory.SecondaryWeapon, SecondaryWeaponSlot, SlotType.SECONDARY_WEAPON);
        }

        if (PrimaryRelicSlot.gameObject.transform.childCount != 1 && PlayerInventory.PrimaryRelic != null)
        {
            AddSlotRelicUi(PlayerInventory.PrimaryRelic, PrimaryRelicSlot, SlotType.PRIMARY_RELIC);
        }

        if (SecondaryRelicSlot.gameObject.transform.childCount != 1 && PlayerInventory.SecondaryRelic != null)
        {
            AddSlotRelicUi(PlayerInventory.SecondaryRelic, SecondaryRelicSlot, SlotType.SECONDARY_RELIC);
        }

        if (TertiaryRelicSlot.gameObject.transform.childCount != 1 && PlayerInventory.TertiaryRelic != null)
        {
            AddSlotRelicUi(PlayerInventory.TertiaryRelic, TertiaryRelicSlot, SlotType.TERTIARY_RELIC);
        }

        if (HeadArmourSlot.gameObject.transform.childCount != 1 && PlayerInventory.HeadArmour != null)
        {
            AddSlotArmourUi(PlayerInventory.HeadArmour, HeadArmourSlot, SlotType.HEAD_ARMOUR);
        }

        if (ChestArmourSlot.gameObject.transform.childCount != 1 && PlayerInventory.ChestArmour != null)
        {
            AddSlotArmourUi(PlayerInventory.ChestArmour, ChestArmourSlot, SlotType.CHEST_ARMOUR);
        }

        if (LegArmourSlot.gameObject.transform.childCount != 1 && PlayerInventory.LegArmour != null)
        {
            AddSlotArmourUi(PlayerInventory.LegArmour, LegArmourSlot, SlotType.LEG_ARMOUR);
        }

        if (FootArmourSlot.gameObject.transform.childCount != 1 && PlayerInventory.FootArmour != null)
        {
            AddSlotArmourUi(PlayerInventory.FootArmour, FootArmourSlot, SlotType.FOOT_ARMOUR);
        }
    }

    public void AddSlotWeaponUi(WeaponItem item, GameObject target, SlotType slot)
    {
        var newItemUi = Instantiate(this.WeaponItemTemplate);
        newItemUi.transform.SetParent(target.transform, false);
        newItemUi.transform.position = target.transform.position;
        newItemUi.DisplayName = item.Name;
        newItemUi.ItemId = item.Id;
        newItemUi.SlotType = slot;
        newItemUi.WeaponDamage = item.WeaponDamage;
        newItemUi.WeaponType = item.WeaponType;
        newItemUi.AmmoLeft = item.AmmoLeft;
        newItemUi.SetController(this);
        newItemUi.SetText();
    }

    public void AddSlotArmourUi(ArmourItem item, GameObject target, SlotType slot)
    {
        var newItemUi = Instantiate(this.ArmourItemTemplate);
        newItemUi.transform.SetParent(target.transform, false);
        newItemUi.transform.position = target.transform.position;
        newItemUi.DisplayName = item.Name;
        newItemUi.ItemId = item.Id;
        newItemUi.SlotType = slot;
        newItemUi.ArmourResistance = item.ArmourResistance;
        newItemUi.ArmourType = item.ArmourType;
        newItemUi.SetController(this);
        newItemUi.SetText();
    }

    public void AddSlotRelicUi(RelicItem item, GameObject target, SlotType slot)
    {
        var newItemUi = Instantiate(this.RelicItemTemplate);
        newItemUi.transform.SetParent(target.transform, false);
        newItemUi.transform.position = target.transform.position;
        newItemUi.DisplayName = item.Name;
        newItemUi.ItemId = item.Id;
        newItemUi.SlotType = slot;
        newItemUi.RelicPower = item.RelicPower;
        newItemUi.RelicType = item.RelicType;
        newItemUi.SetController(this);
        newItemUi.SetText();
    }

    public void AttemptEquip(GameObject targetSlot)
    {
        if (SelectedStacks.Count == 1)
        {
            var stack = SelectedStacks.FirstOrDefault();

            if (stack.ItemData.ItemType == ItemType.WEAPON && stack.ItemData.EquipableType == EquipableType.WEAPON)
            {
                if (targetSlot.name == "PrimaryWeaponSlot")
                {
                    var newItem = stack.ItemData;
                    PlayerInventory.Items.Remove(newItem);
                    var newWeapon = ItemRegistry.ItemToWeapon(newItem);
                    PlayerInventory.PrimaryWeapon = newWeapon;
                    AddSlotWeaponUi(newWeapon, targetSlot, SlotType.PRIMARY_WEAPON);
                }
                else if (targetSlot.name == "SecondaryWeaponSlot")
                {
                    var newItem = stack.ItemData;
                    PlayerInventory.Items.Remove(newItem);
                    var newWeapon = ItemRegistry.ItemToWeapon(newItem);
                    PlayerInventory.SecondaryWeapon = newWeapon;
                    AddSlotWeaponUi(newWeapon, targetSlot, SlotType.SECONDARY_WEAPON);
                }
            }
            else if (stack.ItemData.ItemType == ItemType.ARMOUR)
            {
                if (targetSlot.name == "HeadArmourSlot" && ItemRegistry.ItemToArmour(stack.ItemData).EquipableType == EquipableType.HEAD)
                {
                    var newItem = stack.ItemData;
                    PlayerInventory.Items.Remove(newItem);
                    var newArmour = ItemRegistry.ItemToArmour(newItem);
                    PlayerInventory.HeadArmour = newArmour;
                    AddSlotArmourUi(newArmour, targetSlot, SlotType.HEAD_ARMOUR);
                }
                else if (targetSlot.name == "ChestArmourSlot" && ItemRegistry.ItemToArmour(stack.ItemData).EquipableType == EquipableType.CHEST)
                {
                    var newItem = stack.ItemData;
                    PlayerInventory.Items.Remove(newItem);
                    var newArmour = ItemRegistry.ItemToArmour(newItem);
                    PlayerInventory.ChestArmour = newArmour;
                    AddSlotArmourUi(newArmour, targetSlot, SlotType.CHEST_ARMOUR);
                }
                else if (targetSlot.name == "LegArmourSlot" && ItemRegistry.ItemToArmour(stack.ItemData).EquipableType == EquipableType.LEGS)
                {
                    var newItem = stack.ItemData;
                    PlayerInventory.Items.Remove(newItem);
                    var newArmour = ItemRegistry.ItemToArmour(newItem);
                    PlayerInventory.LegArmour = newArmour;
                    AddSlotArmourUi(newArmour, targetSlot, SlotType.LEG_ARMOUR);
                }
                else if (targetSlot.name == "FootArmourSlot" && ItemRegistry.ItemToArmour(stack.ItemData).EquipableType == EquipableType.FEET)
                {
                    var newItem = stack.ItemData;
                    PlayerInventory.Items.Remove(newItem);
                    var newArmour = ItemRegistry.ItemToArmour(newItem);
                    PlayerInventory.FootArmour = newArmour;
                    AddSlotArmourUi(newArmour, targetSlot, SlotType.FOOT_ARMOUR);
                }
            }
            else if (stack.ItemData.ItemType == ItemType.RELIC)
            {
                if (targetSlot.name == "PrimaryRelicSlot")
                {
                    var newItem = stack.ItemData;
                    PlayerInventory.Items.Remove(newItem);
                    var newRelic = ItemRegistry.ItemToRelic(newItem);
                    PlayerInventory.PrimaryRelic = newRelic;
                    AddSlotRelicUi(newRelic, targetSlot, SlotType.PRIMARY_RELIC);
                }
                else if (targetSlot.name == "SecondaryRelicSlot")
                {
                    var newItem = stack.ItemData;
                    PlayerInventory.Items.Remove(newItem);
                    var newRelic = ItemRegistry.ItemToRelic(newItem);
                    PlayerInventory.SecondaryRelic = newRelic;
                    AddSlotRelicUi(newRelic, targetSlot, SlotType.SECONDARY_RELIC);
                }
                else if (targetSlot.name == "TertiaryRelicSlot")
                {
                    var newItem = stack.ItemData;
                    PlayerInventory.Items.Remove(newItem);
                    var newRelic = ItemRegistry.ItemToRelic(newItem);
                    PlayerInventory.TertiaryRelic = newRelic;
                    AddSlotRelicUi(newRelic, targetSlot, SlotType.TERTIARY_RELIC);
                }
            }
        }

        RefreshItemUiText();
        GeneratePlayerItems();
        PopulatePlayerInv();
        SelectedStacks.Clear();
    }

    public void ClearEquippedUis()
    {
        while (PrimaryWeaponSlot.transform.childCount > 0)
        {
            GameObject.DestroyImmediate(PrimaryWeaponSlot.transform.GetChild(1));
        }

        while (SecondaryWeaponSlot.transform.childCount > 0)
        {
            GameObject.DestroyImmediate(SecondaryWeaponSlot.transform.GetChild(1));
        }

        while (PrimaryRelicSlot.transform.childCount > 0)
        {
            GameObject.DestroyImmediate(PrimaryRelicSlot.transform.GetChild(1));
        }

        while (SecondaryRelicSlot.transform.childCount > 0)
        {
            GameObject.DestroyImmediate(SecondaryRelicSlot.transform.GetChild(1));
        }

        while (TertiaryRelicSlot.transform.childCount > 0)
        {
            GameObject.DestroyImmediate(TertiaryRelicSlot.transform.GetChild(1));
        }

        while (HeadArmourSlot.transform.childCount > 0)
        {
            GameObject.DestroyImmediate(HeadArmourSlot.transform.GetChild(1));
        }

        while (ChestArmourSlot.transform.childCount > 0)
        {
            GameObject.DestroyImmediate(ChestArmourSlot.transform.GetChild(1));
        }

        while (LegArmourSlot.transform.childCount > 0)
        {
            GameObject.DestroyImmediate(LegArmourSlot.transform.GetChild(1));
        }

        while (FootArmourSlot.transform.childCount > 0)
        {
            GameObject.DestroyImmediate(FootArmourSlot.transform.GetChild(1));
        }
    }

    public void RefreshItemUiText()
    {
        foreach (var ui in PlayerItemUis)
        {
            ui.SetText();
        }

        foreach (var ui in TargetItemUis)
        {
            ui.SetText();
        }
    }

    public void UnequipWeapon(GameObject itemUi)
    {
        var ui = itemUi.GetComponent<EquipmentSlotUi>();
        var itemData = ItemRegistry.Weapons.GetById(ui.ItemId);
        itemData.AmmoLeft = ui.AmmoLeft;
        PlayerInventory.Items.Add(itemData);

        if (ui.SlotType == SlotType.PRIMARY_WEAPON)
        {
            PlayerInventory.PrimaryWeapon = null;
        }
        if (ui.SlotType == SlotType.SECONDARY_WEAPON)
        {
            PlayerInventory.SecondaryWeapon = null;
        }

        ui.DeleteSelf();
        GeneratePlayerItems();
        PopulatePlayerInv();
    }

    public void UnequipArmour(GameObject itemUi)
    {
        var ui = itemUi.GetComponent<EquipmentSlotUi>();
        var itemData = ItemRegistry.Armours.GetById(ui.ItemId);
        PlayerInventory.Items.Add(itemData);

        if (ui.SlotType == SlotType.HEAD_ARMOUR)
        {
            PlayerInventory.HeadArmour = null;
        }
        if (ui.SlotType == SlotType.CHEST_ARMOUR)
        {
            PlayerInventory.ChestArmour = null;
        }
        if (ui.SlotType == SlotType.FOOT_ARMOUR)
        {
            PlayerInventory.FootArmour = null;
        }
        if (ui.SlotType == SlotType.LEG_ARMOUR)
        {
            PlayerInventory.LegArmour = null;
        }

        ui.DeleteSelf();
        GeneratePlayerItems();
        PopulatePlayerInv();
    }

    public void UnequipRelic(GameObject itemUi)
    {
        var ui = itemUi.GetComponent<EquipmentSlotUi>();
        var itemData = ItemRegistry.Relics.GetById(ui.ItemId);
        PlayerInventory.Items.Add(itemData);

        if (ui.SlotType == SlotType.PRIMARY_RELIC)
        {
            PlayerInventory.PrimaryRelic = null;
        }
        if (ui.SlotType == SlotType.SECONDARY_RELIC)
        {
            PlayerInventory.SecondaryRelic = null;
        }
        if (ui.SlotType == SlotType.TERTIARY_RELIC)
        {
            PlayerInventory.TertiaryRelic = null;
        }

        ui.DeleteSelf();
        GeneratePlayerItems();
        PopulatePlayerInv();
    }
}
