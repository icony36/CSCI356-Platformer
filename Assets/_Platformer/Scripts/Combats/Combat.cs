using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combat : MonoBehaviour, IDamageable
{
    [field: Header("Settings")]
    [field: SerializeField] public string TargetTag { get; private set; }
    [field: SerializeField] public HitBox AttackHitbox { get; private set; }
    [Header("States")]
    [Tooltip("For testing purpose.")]
    [SerializeField] protected bool isInvincible;

    protected AudioManager audioManager;

    // Public Variables
    public bool CanAttack = true;
    public bool CanSkill = true;

    public abstract void Attack();
    public abstract void CheckIsDead();
    public abstract void InstantKill();
    public abstract void InflictDamage(float damageToInflict, Vector3 damageSource);

    protected virtual void Awake()
    {
        audioManager = AudioManager.Instance;
    }
}
