using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipmentSlotUi : MonoBehaviour
{
    public string DisplayName;
    public string Description;
    public string ItemId;
    public WeaponType WeaponType;
    public ArmourType ArmourType;
    public RelicType RelicType;
    public int WeaponDamage;
    public int ArmourResistance;
    public int RelicPower;
    public int AmmoLeft;
    public SlotType SlotType;
    public List<string> Stats = new List<string>();
    private InventoryController _invController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText()
    {
        for (int i = 0; i < 3; i++)
        {
            var child = this.gameObject.transform.GetChild(i);

            if (child.name == "WeaponName" || child.name == "ArmourName" || child.name == "RelicName")
            {
                child.GetComponent<TextMeshProUGUI>().SetText(this.DisplayName);
            }
            else if (child.name == "WeaponDMG")
            {
                child.GetComponent<TextMeshProUGUI>().SetText(this.WeaponDamage.ToString());
            }
            else if (child.name == "WeaponType")
            {
                child.GetComponent<TextMeshProUGUI>().SetText(this.WeaponType.TypeName);
            }
            else if (child.name == "ArmourRES")
            {
                child.GetComponent<TextMeshProUGUI>().SetText(this.ArmourResistance.ToString());
            }
            else if (child.name == "ArmourType")
            {
                child.GetComponent<TextMeshProUGUI>().SetText(this.ArmourType.TypeName);
            }
            else if (child.name == "RelicPWR")
            {
                child.GetComponent<TextMeshProUGUI>().SetText(this.RelicPower.ToString());
            }
            else if (child.name == "RelicType")
            {
                child.GetComponent<TextMeshProUGUI>().SetText(this.RelicType.TypeName);
            }
        }
    }

    public void Unequip()
    {
        if (ItemRegistry.Weapons.GetById(ItemId) != null)
        {
            _invController.UnequipWeapon(this.gameObject);
        }
        else if (ItemRegistry.Armours.GetById(ItemId) != null)
        {
            _invController.UnequipArmour(this.gameObject);
        }
        else if (ItemRegistry.Relics.GetById(ItemId) != null)
        {
            _invController.UnequipRelic(this.gameObject);
        }
    }

    public void SetController(InventoryController controller)
    {
        _invController = controller;
    }

    public void DeleteSelf()
    {
        GameObject.DestroyImmediate(this.gameObject);
    }
}
