using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VictoryTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent onPassedThrough;

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            if(onPassedThrough != null)
            {
                onPassedThrough.Invoke();
            }
        }
    }
}
