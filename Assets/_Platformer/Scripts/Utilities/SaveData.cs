using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData 
{
    //player health
    public int currentHealth;

    //player transform positon
    public float posX;
    public float posY;
    public float posZ;  
    
    //entity state
    public Dictionary<int, bool> enemySaveState;
    public Dictionary<int, bool> powerupSaveState;
}
