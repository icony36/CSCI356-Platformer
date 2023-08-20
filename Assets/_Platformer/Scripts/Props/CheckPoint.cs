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
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null )
        {
            player.SetLastCheckPoint(gameObject);

            gameManager.SaveGame();

            //sphereCollider.enabled = false;
        }
    }
}
