using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCombat : Combat
{
    [field: SerializeField] public Color TagColor { get; protected set; } = new Color(255, 0, 0);

    [field: Header("Health")]
    [field: SerializeField] public int MaxHealth { get; protected set; } = 100;
    [field: SerializeField] public int CurrentHealth { get; protected set; }

    [field: Header("Normal Attack")]
    [field: SerializeField] public int AttackDamage { get; protected set; } = 10;

    [Header("Shooting")]
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected GameObject damageOrbPrefab;

    [field: Header("Floating Damage Text")]
    [field: SerializeField] protected GameObject damageTextPrefab;

    // References
    protected Bot bot;
    protected AudioManager audioManager;
    protected GameManager gameManager;

    // Local Variables

    protected virtual void Start()
    {
        bot = GetComponent<Bot>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager")?.GetComponent<AudioManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager")?.GetComponent<GameManager>();

        CurrentHealth = MaxHealth;
    }

    public override void InstantKill()
    {
        CurrentHealth = 0;

        CheckIsDead();
    }

    public override void Attack()
    {
        if (!CanAttack) { return; }

        bot.PlayAnimAttack();
    }

    public override void UseSkill()
    {
        if (!CanSkill) { return; }
    }

    public override void CheckIsDead()
    {
        if (CurrentHealth <= 0)
        {
            bot.SwitchBotState(Bot.BotState.Dead);
            gameManager.enemyState[bot.ID] = false;
            
            // play sfx
            audioManager?.PlaySFX("EnemyDeath");
        }
    }

    public override void InflictDamage(float damageToInflict, Vector3 damageSource)
    {
        if (isInvincible) { return; }

        CurrentHealth -= (int)Mathf.Clamp(damageToInflict, 0, damageToInflict);

        bot.PlayAnimHurt();

        // instantiate floating damage
        DamageIndicator indicator = Instantiate(damageTextPrefab, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        indicator.SetDamageText(damageToInflict);

        // play sfx
        // play vfx

        CheckIsDead();
    }

    // Animation Event
    public virtual void AnimEvents_Hit()
    {
        AttackHitbox.EnableHitBox(AttackDamage);

        // play sfx
        audioManager?.PlaySFX("EnemyDamageOrb");

        // play vfx
        bot.EnemyVFXManager.PlaySkillEffect();
    }

    // Animation Event
    public virtual void AnimEvents_HitEnd()
    {
        AttackHitbox.DisableHitBox();
    }

    // Animation Event
    public virtual void AnimEvents_End()
    {
        //bot.SwitchBotState(Bot.BotState.Patrolling);
    }

    // Animation Event
    public virtual void AnimEvents_Shoot()
    {
        Instantiate(damageOrbPrefab, shootingPoint.position, Quaternion.LookRotation(shootingPoint.forward));
        
        // play sfx
        audioManager?.PlaySFX("EnemyDamageOrb");

        // play vfx
        bot.EnemyVFXManager.PlaySkillEffect();
    }
}
