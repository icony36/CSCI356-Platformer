using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVFXManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem healEffect;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private ParticleSystem shootEffect;
    [SerializeField] private ParticleSystem smashEffectPrefab;

    public void PlayHitEffect()
    {       

        if (hitEffect != null)
        {
            hitEffect.Play();
        }
    }

    public void PlayShootEffect()
    {

        if (shootEffect != null)
        {
            shootEffect.Play();
        }
    }

    public void PlayHealEffect()
    {

        if (healEffect != null)
        {
            healEffect.Play();
        }
    }

    public void PlaySmashEffect()
    {
        if (smashEffectPrefab != null)
        {
            ParticleSystem ps = Instantiate(smashEffectPrefab, transform.position + new Vector3(0f, 0.01f, 0f), Quaternion.identity);
            ps.gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
        }
    }
}
