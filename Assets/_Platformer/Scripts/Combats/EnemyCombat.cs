using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCombat : Combat
{
    [field: SerializeField] public Color TagColor { get; private set; } = new Color(255, 0, 0);

    [field: Header("Health")]
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    [field: SerializeField] public int CurrentHealth { get; private set; }

    [field: Header("Normal Attack")]
    [field: SerializeField] public int AttackDamage { get; private set; } = 10;

    [Header("Shooting")]
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject damageOrbPrefab;

    [field: Header("Floating Damage Text")]
    [field: SerializeField] private GameObject damageTextPrefab;

    // References
    private Bot bot;

    // Local Variables

    private void Start()
    {
        bot = GetComponent<Bot>();

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

        // for bot
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
        }
    }

    public override void InflictDamage(float damageToInflict)
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
    public void AnimEvents_Hit()
    {
        AttackHitbox.EnableHitBox(AttackDamage);

        // play sfx
        // play vfx
    }

    // Animation Event
    public void AnimEvents_HitEnd()
    {
        AttackHitbox.DisableHitBox();
    }

    // Animation Event
    public void AnimEvents_End()
    {
        //bot.SwitchBotState(Bot.BotState.Patrolling);
    }

    // Animation Event
    public void AnimEvents_Shoot()
    {
        Instantiate(damageOrbPrefab, shootingPoint.position, Quaternion.LookRotation(shootingPoint.forward));

        // play sfx
        // play vfx
    }
}
