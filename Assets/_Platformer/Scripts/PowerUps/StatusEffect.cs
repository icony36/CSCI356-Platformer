using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerData initData;

    private BuffIndicator buffIndicator;
    private IEnumerator attackEffect;
    private IEnumerator speedEffect;
    private IEnumerator jumpEffect;

    private void Start()
    {
        buffIndicator = GameMenu.Instance?.GetComponent<BuffIndicator>();
    }

    public void ApplyEffect(PowerupType type, float value, float duration)
    {
        if (type == PowerupType.AttackUp)
        {
            if(attackEffect != null)
                StopCoroutine(attackEffect);

            attackEffect = AttackEffect(value, duration);
            StartCoroutine(attackEffect);
        }  
        else if (type == PowerupType.SpeedUp)
        {
            if (speedEffect != null)
                StopCoroutine(speedEffect);

            speedEffect = SpeedEffect(value, duration);
            StartCoroutine(speedEffect);
        }
        else if (type == PowerupType.JumpUp)
        {
            if (jumpEffect != null)
                StopCoroutine(jumpEffect);

            jumpEffect = JumpEffect(value, duration);
            StartCoroutine(jumpEffect);
        }
    }

    public IEnumerator AttackEffect(float value, float duration)
    {
        float elapsedTime = 0f;
              
        buffIndicator?.SetAttackUpRotationFill(0f);

        while (elapsedTime <= duration)
        {
            playerData.attackDamage = initData.attackDamage + (int)value;
            elapsedTime += Time.deltaTime;
            buffIndicator?.SetAttackUpRotationFill(elapsedTime / duration);
            
            yield return null;
        }

        playerData.attackDamage = initData.attackDamage;
        buffIndicator?.SetAttackUpRotationFill(1f);
    }

    public IEnumerator SpeedEffect(float value, float duration)
    {
        float elapsedTime = 0f;

        buffIndicator?.SetSpeedUpRotationFill(0f);

        while (elapsedTime <= duration)
        {
            playerData.currentMoveSpeed = initData.baseMoveSpeed + (int)value;
            elapsedTime += Time.deltaTime;
            buffIndicator?.SetSpeedUpRotationFill(elapsedTime / duration);

            yield return null;
        }

        playerData.currentMoveSpeed = initData.baseMoveSpeed;
        buffIndicator?.SetSpeedUpRotationFill(1f);
    }

    public IEnumerator JumpEffect(float value, float duration)
    {
        float elapsedTime = 0f;

        buffIndicator?.SetJumpUpRotationFill(0f);

        while (elapsedTime <= duration)
        {
            playerData.maxJumps = initData.maxJumps + (int)value;
            elapsedTime += Time.deltaTime;
            buffIndicator?.SetJumpUpRotationFill(elapsedTime / duration);

            yield return null;
        }

        playerData.maxJumps = initData.maxJumps;
        buffIndicator?.SetJumpUpRotationFill(1f);
    }
}
