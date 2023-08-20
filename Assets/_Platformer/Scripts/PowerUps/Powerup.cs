using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PowerupType
{
    HealthUp,
    AttackUp,
    SpeedUp,
    JumpUp
};

public class Powerup : MonoBehaviour
{
    [SerializeField] private PlayerData playerData; //reference to player data
    [SerializeField] private GameObject effectPrefab;

    [SerializeField] private float value;
    [Tooltip("In seconds.")]
    [SerializeField] private float duration;

    [SerializeField] PowerupType powerupType = new PowerupType();
    [SerializeField] BuffIndicator buffIndicator;


    //[SerializeField] private GameObject jumpUpImage;
    //[SerializeField] private GameObject attackUpImage;
    //[SerializeField] private GameObject speedUpImage;

    private void Start()
    {
        //attackUpImage.gameObject.SetActive(false);
        //speedUpImage.gameObject.SetActive(false);
        //jumpUpImage.gameObject.SetActive(false);  
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
            }
            else if(powerupType.Equals(PowerupType.AttackUp))
            {
                GameObject effect = Instantiate(effectPrefab, other.gameObject.GetComponent<Player>().effectHolder);
                effect.GetComponent<StatusEffect>().InitValues(EffectType.AttackUp, value, duration);

                buffIndicator.SetIsAttack(true);
            }
            else if (powerupType.Equals(PowerupType.SpeedUp))
            {
                GameObject effect = Instantiate(effectPrefab, other.gameObject.GetComponent<Player>().effectHolder);
                effect.GetComponent<StatusEffect>().InitValues(EffectType.SpeedUp, value, duration);

                buffIndicator.SetIsSpeed(true);
            }
            else if (powerupType.Equals(PowerupType.JumpUp))
            {
                GameObject effect = Instantiate(effectPrefab, other.gameObject.GetComponent<Player>().effectHolder);
                effect.GetComponent<StatusEffect>().InitValues(EffectType.JumpUp, value, duration);

                buffIndicator.SetIsJump(true);
            }

            // play vfx
            // play sfx

            Destroy(gameObject);
        }
    }
}
