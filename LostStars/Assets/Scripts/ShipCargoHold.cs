using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCargoHold : MonoBehaviour
{
    private List<GameObject> _cargoInHold = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _cargoInHold.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponentInParent<VehicleActivateable>().IsDriven)
        {
            foreach (var child in _cargoInHold)
            {
                child.transform.parent = this.gameObject.transform;
                child.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
        else
        {
            foreach (var child in _cargoInHold)
            {
                child.transform.parent = null;
                child.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.GetComponent<CargoActivateable>())
        {
            _cargoInHold.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.GetComponent<CargoActivateable>())
        {
            _cargoInHold.Remove(other.gameObject);
        }
    }
}
