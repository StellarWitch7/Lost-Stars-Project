using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SynthCommon : NonPlayerCharacter
{
    public float OptimalDistance = 7f;
    public float OptimalDistanceBuffer = 1f;
    public float PositionTolerance = 3f;
    private NavMeshAgent _navAgent;
    private GameObject _target;

    public override void OverrideStart()
    {
        base.OverrideStart();

        this._navAgent = this.GetComponent<NavMeshAgent>();

        this._target = FindObjectOfType<FirstPersonController>().gameObject;

        this.StartCoroutine(this.Think());
    }

    public override void OverrideUpdate()
    {
        base.OverrideUpdate();
    }

    private bool CalculateBufferedPosition(GameObject gameObject, out Vector3 newPosition)
    {
        var selfPosition = this.transform.position;
        var targetPosition = gameObject.transform.position;
        var direction = targetPosition - selfPosition;
        var distance = direction.magnitude;
        direction.Normalize();

        var min = OptimalDistance - OptimalDistanceBuffer;
        var max = OptimalDistance + OptimalDistanceBuffer;
        if (distance > max)
        {
            var adjustedDistance = Math.Abs(distance - OptimalDistance);
            newPosition = selfPosition + direction * adjustedDistance;
            print($"Too far: {distance}, " +
                $"TARGET:{targetPosition.x}, {targetPosition.z}, " +
                $"PREFER:{adjustedDistance}, " +
                $"DIR:{direction}, " +
                $"GOING TO {newPosition.x}, {newPosition.z}");
            return true;
        }

        print($"Do nothing: {distance}");

        newPosition = Vector3.zero;
        return false;
    }

    private bool AttemptAttack()
    {
        return false;
    }

    private IEnumerator Think()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);

            if (this._target != null)
            {
                if (CalculateBufferedPosition(this._target, out var destination))
                {
                    var distance = (destination - this._navAgent.transform.position).magnitude;
                    if (distance > this.PositionTolerance)
                    {
                        this._navAgent.SetDestination(destination);
                    }
                }
                else
                {
                    if (AttemptAttack() == true)
                    {
                        
                    }
                }
            }
        }
    }
}
