using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LadderEnter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Movement movement = other.GetComponent<Movement>();
        if (movement != null)
        {
            movement.SetIsNearLadder(transform);
        }
    }
}
