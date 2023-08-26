using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource audioSource;

    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip attack1SFX;
    [SerializeField] private AudioClip attack2SFX;
    [SerializeField] private AudioClip hitSFX;
    [SerializeField] private AudioClip dashSFX;
    [SerializeField] private AudioClip buffSFX;
    [SerializeField] private AudioClip healthBuffSFX;
    [SerializeField] private AudioClip lightningSkillSFX;
    [SerializeField] private AudioClip enemyDamageOrbSFX;
    [SerializeField] private AudioClip enemyDeathSFX;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(string sfxName)
    {
        switch(sfxName)
        {
            case "Jump":
                audioSource.PlayOneShot(jumpSFX); 
                break;
            case "Attack1":
                audioSource.PlayOneShot(attack1SFX);
                break;
            case "Attack2":
                audioSource.PlayOneShot(attack2SFX);
                break;
            case "Hit":
                audioSource.PlayOneShot(hitSFX);
                break;
            case "Dash":
                audioSource.PlayOneShot(dashSFX, 0.5f);
                break;
            case "Buff":
                audioSource.PlayOneShot(buffSFX);
                break;
            case "HealthBuff":
                audioSource.PlayOneShot(healthBuffSFX);
                break;
            case "PlayerSkill":
                audioSource.PlayOneShot(lightningSkillSFX);
                break;
            case "EnemyDamageOrb":
                audioSource.PlayOneShot(enemyDamageOrbSFX, 0.25f);
                break;
            case "EnemyDeath":
                audioSource.PlayOneShot(enemyDeathSFX, 0.25f);
                break;
            default:
                Debug.Log(sfxName + " sfx not found.");
                break;
        }
    }

    public void StopSFX()
    {
        audioSource.Stop();
    }
}
