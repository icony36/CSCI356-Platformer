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

    private void Start()
    {
        buffIndicator = GameObject.FindGameObjectWithTag("UICanvas")?.GetComponent<BuffIndicator>();
    }

    private void Update()
    {
        
    }

    public void ApplyEffect(PowerupType type, float value, float duration)
    {
        if (type == PowerupType.AttackUp)
            StartCoroutine(AttackEffect(value, duration));
        else if (type == PowerupType.SpeedUp)
            StartCoroutine(SpeedEffect(value, duration));
        else if (type == PowerupType.JumpUp)
            StartCoroutine(JumpEffect(value, duration));
    }

    public IEnumerator AttackEffect(float value, float duration)
    {
        float elapsedTime = 0f;

        playerData.attackDamage += (int)value;
        buffIndicator?.SetAttackUpRotationFill(0f);

        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            buffIndicator?.SetAttackUpRotationFill(elapsedTime / duration);

            yield return null;
        }

        buffIndicator?.SetAttackUpRotationFill(1f);
        playerData.attackDamage = initData.attackDamage;
    }

    public IEnumerator SpeedEffect(float value, float duration)
    {
        float elapsedTime = 0f;

        playerData.currentMoveSpeed += value;
        buffIndicator?.SetSpeedUpRotationFill(0f);

        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            buffIndicator?.SetSpeedUpRotationFill(elapsedTime / duration);

            yield return null;
        }

        buffIndicator?.SetSpeedUpRotationFill(1f);
        playerData.currentMoveSpeed = initData.baseMoveSpeed;
    }

    public IEnumerator JumpEffect(float value, float duration)
    {
        float elapsedTime = 0f;

        playerData.maxJumps += (int)value;
        buffIndicator?.SetJumpUpRotationFill(0f);

        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            buffIndicator?.SetJumpUpRotationFill(elapsedTime / duration);

            yield return null;
        }

        buffIndicator?.SetJumpUpRotationFill(1f);
        playerData.maxJumps = initData.maxJumps;
    }
}
