using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSky : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 1.0f;

    private float rot = 0f;
    
    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", rot);
        rot += Time.deltaTime * rotateSpeed;
    }
}
