using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    [SerializeField] private Color color = Color.blue;
    [SerializeField] private float radius = 0.5f;

    private void OnDrawGizmos()
    {
        // Draw a blue sphere at the transform's position
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
