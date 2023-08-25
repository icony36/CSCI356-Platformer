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
            boss.SwitchBossState(Boss.BossState.Dead);

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
        Debug.Log("Shoot");

        boss.PlayAnimShoot();
    }
    
    public void Smash()
    {
        Debug.Log("Smash");

        boss.PlayAnimSmash();
    }

}
