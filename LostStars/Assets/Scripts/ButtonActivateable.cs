using System;
using UnityEngine;
using UnityEngine.Events;

public class ButtonActivateable : Activateable
{
    public ActivateEvent Pressed;

    public override string ActionText => "Press E to activate";

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public override void Activate(GameObject user)
    {
        this.Pressed?.Invoke(user);
    }
}