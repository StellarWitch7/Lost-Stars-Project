using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activateable : MonoBehaviour
{
    public virtual string ActionText => "Activate";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Activate(GameObject user)
    {

    }
}