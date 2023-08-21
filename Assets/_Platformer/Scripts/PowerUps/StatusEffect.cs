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

    private BuffIndicator buffIndicator;

    private float elapsedTime = 0.0f;

    private void Start()
    {
        buffIndicator = GameObject.FindGameObjectWithTag("UICanvas")?.GetComponent<BuffIndicator>();

        if (effectType.Equals(EffectType.AttackUp))
        {
            playerData.attackDamage += (int)value;
            buffIndicator?.SetAttackUpRotationFill(1f);
        }
        else if (effectType.Equals(EffectType.SpeedUp))
        {
            playerData.currentMoveSpeed += value;
            buffIndicator?.SetSpeedUpRotationFill(1f);
        }
        else if (effectType.Equals(EffectType.JumpUp))
        {
            playerData.maxJumps += (int)value;
            buffIndicator?.SetJumpUpRotationFill(1f);
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime < duration)
        {
            if (effectType.Equals(EffectType.AttackUp))
            {
                buffIndicator?.SetAttackUpRotationFill(1 - (elapsedTime / duration));
            }
            else if (effectType.Equals(EffectType.SpeedUp))
            {
                buffIndicator?.SetSpeedUpRotationFill(1 - (elapsedTime / duration));
            }
            else if (effectType.Equals(EffectType.JumpUp))
            {
                buffIndicator?.SetJumpUpRotationFill(1 - (elapsedTime / duration));
            }
        }
        else
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
