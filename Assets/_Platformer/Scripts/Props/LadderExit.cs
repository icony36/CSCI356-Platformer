using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderExit : MonoBehaviour
{
    [SerializeField] private Transform exitPoint;

    private void OnTriggerEnter(Collider other)
    {
        Movement movement = other.GetComponent<Movement>();
        if (movement != null)
        {
            movement.ExitClimb(exitPoint.position);
        }
    }
}
