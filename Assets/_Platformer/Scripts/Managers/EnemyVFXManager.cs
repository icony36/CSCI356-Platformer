using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVFXManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem skillEffect;
    [SerializeField] private ParticleSystem skillEffectPrefab;

    public void PlaySkillEffect()
    {
        if (skillEffectPrefab != null)
        {
            Instantiate(skillEffectPrefab, transform.position + new Vector3(0f, 0.01f, 0f), Quaternion.identity);
        }

        if (skillEffect != null)
        {
            skillEffect.Play();
        }
    }
}
