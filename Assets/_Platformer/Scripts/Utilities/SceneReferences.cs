using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneReferences : MonoBehaviour
{
    public GameObject enemiesHolder;
    public GameObject powerupHolder;
    public Player player;
    public List<Transform> checkpoints;    
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance != null)
            GameManager.Instance.sceneRef = this;
    }
}
