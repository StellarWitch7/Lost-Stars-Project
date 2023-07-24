using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#if ENABLE_INPUT_SYSTEM
[RequireComponent(typeof(ShipInputs))]
#endif

public class ShipController : MonoBehaviour
{
    public float Speed = 1000;

    private ShipInputs _input;
    private HudController _hudController;
    private Rigidbody _body;
    private GameObject _camera;

    private void Awake()
    {
        if (_camera == null)
        {
            _camera = GameObject.FindGameObjectWithTag("ShipCamera");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<ShipInputs>();
        _hudController = GetComponent<HudController>();
        _body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Torque();
        Strafe();
        Thrust();

        if(_input.isParked)
        {
            _body.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            _body.constraints = RigidbodyConstraints.None;
        }

        if(_input.exit)
        {
            _input.exit = false;
            this.GetComponent<PlayerInput>().enabled = false;
            this.GetComponent<VehicleActivateable>().Pilot.GetComponent<PlayerInput>().enabled = true;
            this.GetComponent<VehicleActivateable>().Pilot.GetComponent<FirstPersonController>().ControlledVehicle = null;
            this.GetComponent<VehicleActivateable>().IsDriven = false;
            this.GetComponent<VehicleActivateable>().Pilot = null;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = true;
            GameObject.FindGameObjectWithTag("ShipCamera").GetComponent<Camera>().enabled = false;
        }
    }

    private void Strafe()
    {
        if (_input.up)
        {
            _body.AddRelativeForce(Vector3.up * Speed, ForceMode.Acceleration);
        }
        if (_input.down)
        {
            _body.AddRelativeForce(Vector3.down * Speed, ForceMode.Acceleration);
        }
        if (_input.right)
        {
            _body.AddRelativeForce(Vector3.right * Speed, ForceMode.Acceleration);
        }
        if (_input.left)
        {
            _body.AddRelativeForce(Vector3.left * Speed, ForceMode.Acceleration);
        }
    }

    private void Torque()
    {
        float roll;
        if (_input.rollRight)
        {
            roll = -10;
        }
        else if (_input.rollLeft)
        {
            roll = 10;
        }
        else
        {
            roll = 0;
        }

        var torqueRot = new Vector3(-_input.look.y, _input.look.x, roll);
        _body.AddRelativeTorque(torqueRot * 100f);
    }

    private void Thrust()
    {
        _body.AddRelativeForce(Vector3.forward * _input.speed * Speed);
    }
        
    void LateUpdate()
    {
        
    }
}
