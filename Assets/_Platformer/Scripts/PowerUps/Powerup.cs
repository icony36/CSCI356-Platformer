using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.EditorTools;
using UnityEngine;

public enum PowerupType
{
    HealthUp,
    AttackUp,
    SpeedUp,
    JumpUp
};

public class Powerup : MonoBehaviour
{
    public int ID;
    public bool pickedUp = false;

    [SerializeField] private PlayerData playerData; //reference to player data

    [SerializeField] private float value;
    [Tooltip("In seconds.")]
    [SerializeField] private float duration;

    [SerializeField] PowerupType powerupType = new PowerupType();

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager")?.GetComponent<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(powerupType.Equals(PowerupType.HealthUp))
            {
                if(playerData.currentHealth + value > playerData.maxHealth) 
                    playerData.currentHealth = playerData.maxHealth;
                else
                    playerData.currentHealth += (int)value;

                // play sfx
                audioManager?.PlaySFX(6);
            }
            else
            {
                other.gameObject.GetComponent<StatusEffect>().ApplyEffect(powerupType, value, duration);

                // play sfx
                audioManager?.PlaySFX(5);
            }

            // play vfx
            // play sfx
            GameManager.Instance.powerUpState[ID] = true;
            Destroy(gameObject);
        }
    }
}
