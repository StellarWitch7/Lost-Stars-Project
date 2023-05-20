using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
[Inspectable]
[AddComponentMenu("CurrentProject/Enemy")]
public class AngryBox : MonoBehaviour
{
    [SerializeField, Inspectable]
    public bool IsSearching;

    [SerializeField, Inspectable]
    public Material CalmMaterial;

    [SerializeField, Inspectable]
    public Material AngryMaterial;

    private Variables _variables;
    private MeshRenderer _headMeshRenderer;

    [SerializeField]
    public GameObject Head;

    [Inspectable]
    public UnityEvent DoSomethingElse;

    private float _angle;

    // Start is called before the first frame update
    void Start()
    {
        this._headMeshRenderer = this.Head.GetComponent<MeshRenderer>();
        this._headMeshRenderer.material = this.CalmMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        this._angle = (float)Math.Cos(Time.realtimeSinceStartup / 2) * 45;
        this.Head.transform.localRotation = Quaternion.Euler(0, this._angle, 0);
    }

    public void DisplayAngry()
    {
        this._headMeshRenderer.material = this.AngryMaterial;
    }

    public void DisplayCalm()
    {
        this._headMeshRenderer.material = this.CalmMaterial;
    }
}
