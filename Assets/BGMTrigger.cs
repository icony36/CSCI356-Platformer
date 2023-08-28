using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BGMTrigger : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player") && gameManager != null)
        {
            gameManager.SetIsFightingBoss(false);
        }
    }
}
