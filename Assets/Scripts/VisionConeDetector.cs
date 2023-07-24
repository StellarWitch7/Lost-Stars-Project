using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
[Inspectable]
public class VisionConeDetector : MonoBehaviour
{
    [Inspectable]
    [HideInInspector]
    public Player ClosestPlayer;

    [Inspectable]
    [HideInInspector]
    public bool CanSeePlayer = false;

    [SerializeField]
    public float ViewSize = 90;

    [SerializeField]
    public float ViewDistance = 10;

    [SerializeField]
    public bool ShowDebug = false;

    // Start is called before the first frame update
    void Start()
    {
        var collider = this.GetComponent<SphereCollider>();
        collider.radius = this.ViewDistance;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(this.transform.position, this.transform.position + (this.transform.forward * 10), Color.red, 1);

        if (this.ClosestPlayer != null)
        {
            var vectorToPlayer = this.ClosestPlayer.transform.position - this.transform.position;
            var distance = vectorToPlayer.magnitude;
            var halfViewSize = this.ViewSize / 2;

            var angle = Vector3.Angle(this.transform.forward, vectorToPlayer);
            this.CanSeePlayer = Math.Abs(angle) < halfViewSize;
        }
        else
        {
            this.CanSeePlayer = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Player>();

        this.ClosestPlayer = player;
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if (player != null && this.ClosestPlayer == player)
        {
            this.ClosestPlayer = null;
        }
    }
}
