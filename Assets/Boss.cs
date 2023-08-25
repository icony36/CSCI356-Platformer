using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Bot
{
    [Header("Heal")]
    [SerializeField] private int healCount = 1;

    [Header("Shoot")]
    [Tooltip("In seconds.")]
    [SerializeField] private float shootCooldown = 5f;
    [SerializeField] private int shootCount = 5;

    [Header("Smash")]
    [Tooltip("In seconds.")]
    [SerializeField] private float smashCooldown = 10f;
    [SerializeField] private int smashCount = 1;

    // Animation Params
    private const string ANIM_ATTACK = "Attack";
    private const string ANIM_ATTACK_SPEED = "AttackSpeed";
    private const string ANIM_SPEED = "Speed";
    private const string ANIM_SHOOT = "Shoot";
    private const string ANIM_SMASH = "Smash";
    private const string ANIM_HEAL = "Heal";
    private const string ANIM_HURT = "Hurt";
    private const string ANIM_DEAD = "Dead";
    private const string ANIM_VICTORY = "Victory";
    
    // Referencse
    private BossCombat bossCombat;
    private Collider coliider;

    // Variables 
    private BossState currentBossState;
    private int healCounter = 0;
    private int shootCounter = 0;
    private int smashCounter = 0;
    private float lastShootTime = 0f;
    private float lastSmashTime = 0f;

    // State Machine
    public enum BossState
    {
        Idle,
        Attacking,
        Shooting,
        Smashing,
        Healing,
        Dead
    }

    protected override void Awake()
    {
        base.Awake();

        coliider = GetComponent<Collider>();

        bossCombat = (BossCombat)combat;

        currentBossState = BossState.Idle;
   }

    private void Update()
    {
        if (CheckIsTargetDead())
        {
            PlayAnimVictory();

            return;
        }

        switch (currentBossState)
        {
            case BossState.Idle:
                Idle();
                break;
            case BossState.Attacking:
                Attacking();
                break;
            case BossState.Shooting:
                Shooting();
                break;
            case BossState.Smashing:
                Smashing();
                break;
            case BossState.Healing:
                Healing();
                break;
            case BossState.Dead:
                return;
            default:
                break;
        }

        if (CheckShouldHeal())
        {
            SwitchBossState(BossState.Healing);
        }
        else if (CheckShouldShoot())
        {
            SwitchBossState(BossState.Shooting);
        }
        else if (CheckShouldSmash())
        {
            SwitchBossState(BossState.Smashing);
        }
    }

    public void SwitchBossState(BossState newState)
    {
        // exiting current state
        switch (currentBossState)
        {
            case BossState.Idle:
                break;
            case BossState.Attacking:
                break;
            case BossState.Shooting:
                animator.ResetTrigger(ANIM_SHOOT);
                break;
            case BossState.Smashing:
                animator.ResetTrigger(ANIM_SMASH);
                break;
            case BossState.Healing:
                animator.ResetTrigger(ANIM_HEAL);
                break;
            case BossState.Dead:
                animator.ResetTrigger(ANIM_DEAD);
                //coliider.enabled = true;
                break;
            default:
                break;
        }     

        currentBossState = newState;

        // entering new state
        switch (currentBossState)
        {          
            case BossState.Dead:
                animator.SetTrigger(ANIM_DEAD);
                coliider.enabled = false;
                break;
            default:
                break;
        }
    }

    private void Idle()
    {
        PlayAnimIdle();
        
        if(CanSeeTarget())
        {
            SwitchBossState(BossState.Attacking);
        }
    }

    private void Attacking()
    {
        if (Vector3.Distance(target.transform.position, transform.position) < visionRange)
        {
            if (Vector3.Distance(target.transform.position, transform.position) < attackRange)
            {
                PlayAnimIdle();

                if (CheckShouldAttack())
                {
                    RotateToTarget();
                    bossCombat.Attack();
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                MoveToTarget(target.transform.position);
            }
        }
        else
        {
            // switch back to idle after 1 second
            StartCoroutine(DelaySwitchToIdle(1));
        }
    }

    private void Shooting()
    {
        RotateToTarget();

        bossCombat.Shoot();

        shootCounter++;

        if (shootCounter >= shootCount) 
        {
            lastShootTime = Time.time;
        }
    }

    private void Smashing()
    {
        RotateToTarget();

        bossCombat.Smash();

        smashCounter++;

        if (smashCounter >= smashCount)
        {
            lastSmashTime = Time.time;
        }
    }

    private void Healing()
    {
        bossCombat.Heal();

        healCounter++;
    }

    private bool CheckShouldHeal()
    {
        float healthPercent = (float)bossCombat.CurrentHealth / bossCombat.MaxHealth;

        Debug.Log(healthPercent);

        if (healthPercent <= 0.5f && healCounter < healCount)
        {
            return true;
        }

        return false;
    }

    private bool CheckShouldAttack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            return true;
        }

        return false;
    }

    private bool CheckShouldShoot()
    {
        if (Time.time - lastShootTime >= shootCooldown)
        {
            return true;
        }

        return false;
    }

    private bool CheckShouldSmash()
    {
        if (Time.time - lastSmashTime >= smashCooldown)
        {
            return true;
        }

        return false;
    }

    private IEnumerator DelaySwitchToIdle(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        SwitchBossState(BossState.Idle);
    }

    public override void PlayAnimAttack()
    {
        animator.SetTrigger(ANIM_ATTACK);
        animator.SetFloat(ANIM_ATTACK_SPEED, attackRate);
    }

    public override void PlayAnimIdle()
    {
        animator.SetFloat(ANIM_SPEED, 0f, 0.1f, Time.deltaTime);
    }

    public override void PlayAnimHurt()
    {
        animator.SetTrigger(ANIM_HURT);
    }

    public void PlayAnimHeal()
    {
        animator.SetTrigger(ANIM_HEAL);
    }

    public void PlayAnimShoot()
    {
        animator.SetTrigger(ANIM_SHOOT);
    }

    public void PlayAnimSmash()
    {
        animator.SetTrigger(ANIM_SMASH);
    }

    public void PlayAnimVictory()
    {
        animator.SetTrigger(ANIM_VICTORY);
    }

    // Animation Events
}
