using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellZone : MonoBehaviour
{
    private List<GameObject> _cargoInZone = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoSale(GameObject user)
    {
        OnSell(user);
    }

    public void OnSell(GameObject seller)
    {
        foreach (var cargo in _cargoInZone)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().CreditPlayer(seller, cargo.GetComponent<CargoActivateable>().CargoValue);
            Destroy(cargo.gameObject);
        }

        _cargoInZone.Clear();
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.GetComponent<CargoActivateable>())
        {
            _cargoInZone.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.GetComponent<CargoActivateable>())
        {
            _cargoInZone.Remove(other.gameObject);
        }
    }
}