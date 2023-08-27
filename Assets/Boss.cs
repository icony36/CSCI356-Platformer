using System;
using System.Collections;
using UnityEngine;

public class Boss : Bot
{
    public BossVFXManager BossVFXManager { get; protected set; }

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
    private const string ANIM_SHOOT = "Shoot";
    private const string ANIM_SMASH = "Smash";
    private const string ANIM_HEAL = "Heal";
    private const string ANIM_VICTORY = "Victory";
    
    // Referencse
    private BossCombat bossCombat;
    

    // Variables 
    private int healCounter = 0;
    private int shootCounter = 0;
    private int smashCounter = 0;
    private float lastShootTime = 0f;
    private float lastSmashTime = 0f;

   
    protected override void Awake()
    {
        base.Awake();

        BossVFXManager = GetComponent<BossVFXManager>();

        bossCombat = (BossCombat)combat;
   }

    protected override void Patrolling()
    {
        if (target == null) { return; }

        // chase target if target is within sight and agro range
        if (CanSeeTarget())
        {
            currentState = BotState.Chasing;
        }
        else if (Vector3.Distance(startingPosition, transform.position) > 1f)
        {
            MoveToTarget(startingPosition);
        }
        else
        {
            transform.rotation = startingRotation;
            PlayAnimIdle();
        }
    }

    protected override void Chasing()
    {
        if (target == null) { return; }

        if (Vector3.Distance(target.transform.position, transform.position) < visionRange)
        {
            if (CheckShouldHeal())
            {
                bossCombat.Heal();
            }
            else if (CheckShouldShoot())
            {
                RotateToTarget();

                bossCombat.Shoot();
 
            }
            else if (Vector3.Distance(target.transform.position, transform.position) < attackRange)
            {
                Debug.Log("In attack range");
                
                PlayAnimIdle();
  
                if (CheckShouldSmash())
                {
                    RotateToTarget();

                    bossCombat.Smash();  
                }
                else if (CheckShouldAttack())
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
            // switch back to patrolling after 1 second
            StartCoroutine(DelaySwitchToPatrolling(1));
        }
    }
    

    private bool CheckShouldHeal()
    {
        float healthPercent = (float)bossCombat.CurrentHealth / bossCombat.MaxHealth;

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

    public void HandleHealEnd()
    {
        healCounter++;
    }

    public void HandleShootEnd()
    {
        shootCounter++;

        if (shootCounter > shootCount - 1)
        {
            lastShootTime = Time.time;
            shootCounter = 0;
        }
    }

    public void HandleSmashEnd()
    {
        smashCounter++;

        if (smashCounter > smashCount - 1)
        {
            lastSmashTime = Time.time;
            smashCounter = 0;
        }
    }

    public override void PlayAnimIdle()
    {
        animator.SetFloat(ANIM_SPEED, 0f, 0.1f, Time.deltaTime);
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
}
