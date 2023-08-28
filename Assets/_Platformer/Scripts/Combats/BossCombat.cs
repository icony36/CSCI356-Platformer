using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : EnemyCombat
{
    private Boss boss;

    [Header("Smash")]
    [SerializeField] private int smashDamage;
    [SerializeField] private HitBox smashHitBox;

    protected override void Start()
    {
        base.Start();

        boss = (Boss)bot;
    }

    public override void CheckIsDead()
    {
        if (CurrentHealth <= 0)
        {
            boss.SwitchBotState(Bot.BotState.Dead);

            // play sfx
            audioManager?.PlaySFX("EnemyDeath");
        }
    }

    public void Heal()
    {
        CurrentHealth = MaxHealth;

        // play sfx
        audioManager.PlaySFX("EnemyHeal");

        // play vfx
        boss.BossVFXManager.PlayHealEffect();

        boss.PlayAnimHeal();
    }

    public void Shoot()
    {
        boss.PlayAnimShoot();
    }
    
    public void Smash()
    {
        boss.PlayAnimSmash();
    }

    // Animation Event
    public override void AnimEvents_Hit()
    {
        AttackHitbox.EnableHitBox(AttackDamage);

        // play sfx
        audioManager?.PlaySFX("Punch");

        // play vfx
        boss.BossVFXManager.PlayHitEffect();
    }

    // Animation Event
    public virtual void AnimEvents_Smash()
    {
        smashHitBox.EnableHitBox(smashDamage);

        // play sfx
        audioManager?.PlaySFX("Smash");

        // play vfx
        boss.BossVFXManager.PlaySmashEffect();
    }

    // Animation Event
    public virtual void AnimEvents_SmashEnd()
    {
        smashHitBox.DisableHitBox();

        boss.HandleSmashEnd();
    }

    // Animation Event
    public virtual void AnimEvents_Heal()
    {        
        boss.HandleHealEnd();
    }

    // Animation Event
    public override void AnimEvents_Shoot()
    {
        Instantiate(damageOrbPrefab, shootingPoint.position, Quaternion.LookRotation(shootingPoint.forward));

        // play sfx
        audioManager?.PlaySFX("EnemyDamageOrb");

        // play vfx
        boss.BossVFXManager.PlayShootEffect();
    }

    // Animation Event
    public virtual void AnimEvents_ShootEnd()
    {
        boss.HandleShootEnd();
    }
}
