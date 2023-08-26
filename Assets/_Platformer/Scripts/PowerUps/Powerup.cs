using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private PlayerData playerData; //reference to player data

    [SerializeField] private float value;
    [Tooltip("In seconds.")]
    [SerializeField] private float duration;

    [SerializeField] PowerupType powerupType = new PowerupType();

    private AudioManager audioManager;
    private GameManager gameManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager")?.GetComponent<AudioManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager")?.GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(powerupType.Equals(PowerupType.HealthUp))
            {
                if(playerData.currentHealth + value > playerData.maxHealth)
                {
                    playerData.currentHealth = playerData.maxHealth;
                }
                else
                {
                    playerData.currentHealth += (int)value;
                }

                // play sfx
                audioManager?.PlaySFX("HealthBuff");
            }
            else
            {
                other.gameObject.GetComponent<StatusEffect>().ApplyEffect(powerupType, value, duration);

                // play sfx
                audioManager?.PlaySFX("Buff");
            }

            gameManager.powerUpState[ID] = false;
            gameObject.SetActive(false);
        }
    }
}
