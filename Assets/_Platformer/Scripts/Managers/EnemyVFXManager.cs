using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVFXManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem skillEffect;

    public void PlaySkillEffect()
    {
        if(skillEffect != null)
        {
            Instantiate(skillEffect, transform.position, Quaternion.identity);
        }
    }
}
