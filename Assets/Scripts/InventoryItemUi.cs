using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUi : MonoBehaviour
{
    public string DisplayName;
    public string Description;
    public int Amount;
    public string ItemId;
    public bool IsSelected;
    public ItemStack ItemStack;
    public Inventory CurrentInventory;
    public List<string> Stats = new List<string>();
    private InventoryController _invController;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void SetText()
    {
        for(int i = 0; i < 3; i++)
        {
            var child = this.gameObject.transform.GetChild(i);

            if (child.name == "ItemName")
            {
                child.GetComponent<TextMeshProUGUI>().SetText(this.DisplayName);
            }
            else if (child.name == "StackCount")
            {
                child.GetComponent<TextMeshProUGUI>().SetText(this.Amount.ToString());
            }
            else if (child.name == "ItemSprite")
            {

            }
        }
    }

    public void SelectThisStack()
    {
        if (IsSelected)
        {
            _invController.UpdateStackSelection(ItemStack, false);
            IsSelected = false;
        }
        else
        {
            _invController.UpdateStackSelection(ItemStack, true);
            IsSelected = true;
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