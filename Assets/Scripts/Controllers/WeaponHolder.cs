using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public GameObject Player;
    public WeaponItem EquippedWeapon;
    public GameObject HeldWeapon;
    private bool _weaponChanged;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EquippedWeapon != null)
        {
            if (HeldWeapon == null || _weaponChanged == true)
            {
                SpawnWeapon();
            }
        }
    }

    public void SetWeaponChanged()
    {
        _weaponChanged = true;
    }

    public void SpawnWeapon()
    {
        if (HeldWeapon != null)
        {
            if (HeldWeapon.GetComponent<WeaponObject>().WeaponItem != EquippedWeapon || EquippedWeapon == null)
            {
                //maybe put something here
                GameObject.DestroyImmediate(HeldWeapon);
            }
        }

        if (EquippedWeapon != null)
        {
            HeldWeapon = Instantiate(Resources.Load<GameObject>("Prefabs/Weapons/" + EquippedWeapon.ObjectTemplateName));
            HeldWeapon.transform.SetParent(this.gameObject.transform);
            HeldWeapon.GetComponent<WeaponObject>().SetInv(Player.GetComponent<FirstPersonController>().InventoryController.PlayerInventory);
            HeldWeapon.GetComponent<WeaponObject>().SetPlayer(Player);
            HeldWeapon.GetComponent<WeaponObject>().RefreshAmmoUi();
            HeldWeapon.GetComponent<WeaponObject>().SetAmmoLeft(EquippedWeapon.AmmoLeft);
            HeldWeapon.transform.localPosition = new Vector3(0, 0, 0);
            HeldWeapon.transform.localRotation = Quaternion.identity;
        }

        _weaponChanged = false;
    }
}
