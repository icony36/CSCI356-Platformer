using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    AttackUp,
    SpeedUp,
    JumpUp
};

public class StatusEffect : MonoBehaviour
{
    [SerializeField] EffectType effectType = new EffectType();
    [SerializeField] private float value;
    [SerializeField] private float duration;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerData initData;

    private float elapsedTime = 0.0f;

    private void Start()
    {
        if (effectType.Equals(EffectType.AttackUp))
        {
            playerData.attackDamage += (int)value;
        }
        else if (effectType.Equals(EffectType.SpeedUp))
        {
            playerData.currentMoveSpeed += value;
        }
        else if (effectType.Equals(EffectType.JumpUp))
        {
            playerData.maxJumps += (int)value;
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        
        if(elapsedTime >= duration) 
        {
            if (effectType.Equals(EffectType.AttackUp))
            {
                playerData.attackDamage = initData.attackDamage;
            }
            else if (effectType.Equals(EffectType.SpeedUp))
            {
                playerData.currentMoveSpeed = initData.baseMoveSpeed;
            }
            else if (effectType.Equals(EffectType.JumpUp))
            {
                playerData.maxJumps = initData.maxJumps;
            }

            Destroy(gameObject);
        }
    }

    public void InitValues(EffectType type, float v, float d)
    {
        effectType = type;
        value = v;
        duration = d;
    }
}
