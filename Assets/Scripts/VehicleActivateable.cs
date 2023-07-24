using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleActivateable : Activateable
{
    private string _activateText;
    public bool IsDriven;
    public GameObject Pilot;

    public ActivateEvent Interact;

    public override string ActionText => _activateText;

    public void Start()
    {
        IsDriven = false;
    }

    public void Update()
    {
        if (!IsDriven)
        {
            _activateText = "Press E to drive";
        }
        else
        {
            _activateText = "";
        }
    }

    public override void Activate(GameObject user)
    {
        this.Interact?.Invoke(user);
    }

    public void DoEnter(GameObject user)
    {
        EnterVehicle(user);
    }

    public void EnterVehicle(GameObject user)
    {
        user.GetComponent<FirstPersonController>().ControlledVehicle = this.gameObject;
        Pilot = user;
        IsDriven = true;
        user.GetComponent<PlayerInput>().enabled = false;
        this.gameObject.GetComponent<PlayerInput>().enabled = true;
        GameObject.FindGameObjectWithTag("ShipCamera").GetComponent<Camera>().enabled = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = false;
    }
}