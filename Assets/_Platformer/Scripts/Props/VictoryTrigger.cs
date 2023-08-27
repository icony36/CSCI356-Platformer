using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VictoryTrigger : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            gameManager.GameIsFinished();
        }
    }
}
