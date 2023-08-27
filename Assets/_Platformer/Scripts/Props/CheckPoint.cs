using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CheckPoint : MonoBehaviour
{
    private SphereCollider sphereCollider;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance.GetComponent<GameManager>();
        
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null )
        {
            player.SetLastCheckPoint(gameObject);

            gameManager.SaveData();
        }
    }
}
