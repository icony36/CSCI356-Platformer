using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float magnitude = 0.5f;

    private void Update()
    {
        float deltaY = Mathf.Sin(Time.time * speed) * Time.deltaTime * magnitude;

        transform.position = new Vector3(transform.position.x, transform.position.y + deltaY, transform.position.z);
    }
}
