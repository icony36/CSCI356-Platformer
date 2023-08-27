using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : EnemyCombat
{
    private Boss boss;

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
    public virtual void AnimEvents_HealEnd()
    {
        boss.HandleHealEnd();
    }

    // Animation Event
    public virtual void AnimEvents_SmashEnd()
    {
        boss.HandleSmashEnd();
    }

    // Animation Event
    public virtual void AnimEvents_ShootEnd()
    {
        boss.HandleShootEnd();
    }
}
