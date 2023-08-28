using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : GenericSingleton<AudioManager>
{
    private AudioSource audioSource;

    [Header("Game")]
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
    [SerializeField] private AudioClip enemyHealSFX;
    [SerializeField] private AudioClip punchSFX;
    [SerializeField] private AudioClip smashSFX;
    [SerializeField] private AudioClip hurtSFX;

    [Header("UI")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private AudioClip toggleSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(string sfxName)
    {
        switch(sfxName)
        {
            case "Jump":
                audioSource.PlayOneShot(jumpSFX, 2f); 
                break;
            case "Attack1":
                audioSource.PlayOneShot(attack1SFX, 2f);
                break;
            case "Attack2":
                audioSource.PlayOneShot(attack2SFX, 2f);
                break;
            case "Hit":
                audioSource.PlayOneShot(hitSFX, 2f);
                break;
            case "Dash":
                audioSource.PlayOneShot(dashSFX, 2f);
                break;
            case "Buff":
                audioSource.PlayOneShot(buffSFX, 0.5f);
                break;
            case "HealthBuff":
                audioSource.PlayOneShot(healthBuffSFX);
                break;
            case "PlayerSkill":
                audioSource.PlayOneShot(lightningSkillSFX, 2f);
                break;
            case "EnemyDamageOrb":
                audioSource.PlayOneShot(enemyDamageOrbSFX, 0.25f);
                break;
            case "EnemyDeath":
                audioSource.PlayOneShot(enemyDeathSFX);
                break;
            case "EnemyHeal":
                audioSource.PlayOneShot(enemyHealSFX);
                break;
            case "Punch":
                audioSource.PlayOneShot(punchSFX);
                break;
            case "Smash":
                audioSource.PlayOneShot(smashSFX, 0.5f);
                break;
            case "Hurt":
                audioSource.PlayOneShot(hurtSFX);
                break;
            default:
                Debug.Log(sfxName + " sfx not found.");
                break;
        }
    }

    public void PlaySFX(string sfxName, Vector3 position)
    {
        switch (sfxName)
        {
            case "Jump":
                AudioSource.PlayClipAtPoint(jumpSFX, position, 2f);
                break;
            case "Attack1":
                AudioSource.PlayClipAtPoint(attack1SFX, position, 2f);
                break;
            case "Attack2":
                AudioSource.PlayClipAtPoint(attack2SFX, position, 2f);
                break;
            case "Hit":
                AudioSource.PlayClipAtPoint(hitSFX, position, 4f);
                break;
            case "Dash":
                AudioSource.PlayClipAtPoint(dashSFX, position, 2f);
                break;
            case "Buff":
                AudioSource.PlayClipAtPoint(buffSFX, position, 0.5f);
                break;
            case "HealthBuff":
                AudioSource.PlayClipAtPoint(healthBuffSFX, position);
                break;
            case "PlayerSkill":
                AudioSource.PlayClipAtPoint(lightningSkillSFX, position, 2f);
                break;
            case "EnemyDamageOrb":
                AudioSource.PlayClipAtPoint(enemyDamageOrbSFX, position, 0.25f);
                break;
            case "EnemyDeath":
                AudioSource.PlayClipAtPoint(enemyDeathSFX, position);
                break;
            case "EnemyHeal":
                AudioSource.PlayClipAtPoint(enemyHealSFX, position);
                break;
            case "Punch":
                AudioSource.PlayClipAtPoint(punchSFX, position);
                break;
            case "Smash":
                AudioSource.PlayClipAtPoint(smashSFX, position, 0.5f);
                break;
            case "Hurt":
                AudioSource.PlayClipAtPoint(hurtSFX, position);
                break;
            default:
                Debug.Log(sfxName + " sfx not found.");
                break;
        }
    }

    public void PlayUISound(string sound)
    {
        switch (sound)
        {
            case "Hover":
                audioSource.PlayOneShot(hoverSound, 2f);
                break;
            case "Click":
                audioSource.PlayOneShot(clickSound, 2f);
                break;
            case "Close":
                audioSource.PlayOneShot(closeSound, 4f);
                break;
            case "Toggle":
                audioSource.PlayOneShot(toggleSound, 2f);
                break;
            default:
                Debug.Log(sound + " sfx not found.");
                break;
        }
    }

    public void StopSFX()
    {
        audioSource.Stop();
    }
}
