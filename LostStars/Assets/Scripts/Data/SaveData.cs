using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int SaveSlot;
    public PlayerInventory Inventory;

    public float CurrentHealth;
    public float MaxHealth;

    public float PositionX;
    public float PositionY;
    public float PositionZ;

    public SaveData(FirstPersonController player)
    {
        SaveSlot = GameObject.Find("GameManager").GetComponent<GameManager>().SaveSlot;
        Inventory = player.InventoryController.PlayerInventory;

        CurrentHealth = player.GetCurrentHealth();
        MaxHealth = player.GetMaxHealth();

        PositionX = player.gameObject.transform.position.x;
        PositionY = player.gameObject.transform.position.y;
        PositionZ = player.gameObject.transform.position.z;
    }
}
