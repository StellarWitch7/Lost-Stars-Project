using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int SaveSlot;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreditPlayer(GameObject player, float amount)
    {
        if (player.GetComponent<FirstPersonController>())
        {
            player.GetComponent<FirstPersonController>().InventoryController.PlayerInventory.Credits += amount;
        }
        else
        {
            print("Player not found");
        }
    }
}
