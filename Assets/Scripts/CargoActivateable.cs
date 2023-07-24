using StarterAssets;
using UnityEngine;

public class CargoActivateable : Activateable
{
    public bool IsPickedUp;
    public Vector3 lockPoint;
    private string _activateText;
    public float CargoValue;
    public GameObject LastCarrier;

    public ActivateEvent Interact;

    public override string ActionText => _activateText;

    public void Start()
    {
        IsPickedUp = false;
        CargoValue = Random.Range(100, 1000);
    }

    public void Update()
    {
        if(IsPickedUp)
        {
            //Change this to set the pickup point as parent
            lockPoint = LastCarrier.GetComponent<FirstPersonController>().PickupPoint;
            this.gameObject.transform.position = lockPoint;
            _activateText = "Press E to drop";
        }
        else
        {
            _activateText = "Press E to pick up";
        }
    }

    public override void Activate(GameObject user)
    {
        this.Interact?.Invoke(user);
    }

    public void DoPickDrop(GameObject user)
    {
        if (IsPickedUp)
        {
            OnDrop(user);
        }
        else
        {
            OnPickUp(user);
        }
    }

    public void OnPickUp(GameObject carrier)
    {
        FirstPersonController fpc = carrier.GetComponent<FirstPersonController>();
        IsPickedUp = true;
        LastCarrier = carrier;
        fpc.IsCarrying = true;
        fpc.ItemPickedUp = this;
    }

    public void OnDrop(GameObject carrier)
    {
        FirstPersonController fpc = carrier.GetComponent<FirstPersonController>();
        IsPickedUp = false;
        fpc.IsCarrying = false;
        fpc.ItemPickedUp = null;
    }

    public void Zap(GameObject user)
    {

    }
}
