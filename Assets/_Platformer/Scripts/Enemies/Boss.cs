using System;
using System.Collections;
using UnityEngine;

public class Boss : Bot
{
    public BossVFXManager BossVFXManager { get; protected set; }

    [Header("Patrolling")]
    [SerializeField] protected Transform patrolStartPoint;

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
    private GameManager gameManager;

    // Variables 
    private Vector3 patrolStartPosition;
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

        gameManager = GameManager.Instance;

        patrolStartPosition = patrolStartPoint.position;
    }

    public override void SwitchBotState(BotState newState)
    {
        if (newState == BotState.Chasing)
        {
            gameManager.SetIsFightingBoss(true);
        }

        base.SwitchBotState(newState);
    }

    protected override void Patrolling()
    {
        if (target == null) { return; }

        // chase target if target is within agro range (eyes on back)
        if (IsWithinAgroRange() && IsTargetSameHeight())
        {
            SwitchBotState(BotState.Chasing);
        }
        else if (Vector3.Distance(startingPosition, transform.position) > 0.1f) // back to starting position if not at starting position
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

        if (!IsWithinAgroRange() || !IsTargetSameHeight())
        {
            // switch back to patrolling after 1 second
            StartCoroutine(DelaySwitchToPatrolling(1));

            return;
        }

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

    protected override void MoveToTarget(Vector3 targetPosition)
    {
        if (Vector3.Distance(targetPosition, transform.position) < 0.1f)
        {
            PlayAnimIdle();

            return;
        }

        // get movement direction
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;
        direction.z = 0;

        // rotate to target
        transform.rotation = Quaternion.LookRotation(direction);

        // constraint movement between starting position and ending position
        float maxX = Mathf.Max(patrolStartPosition.x, endingPosition.x);
        float minX = Mathf.Min(patrolStartPosition.x, endingPosition.x);
        if (transform.position.x <= maxX && transform.position.x >= minX)
        {
            // move forward
            transform.Translate(0, 0, 0.2f * Time.deltaTime * speed);
        }
        else // relocate to nearest waypoint if out of boundary
        {
            float distFromStart = Vector3.Distance(transform.position, patrolStartPosition);
            float distFromEnd = Vector3.Distance(transform.position, endingPosition);

            Vector3 nearestPoint = distFromStart < distFromEnd ? patrolStartPosition : endingPosition;

            transform.position = nearestPoint;
        }

        animator.SetFloat(ANIM_SPEED, 1, 0.1f, Time.deltaTime);
    }

    private bool IsWithinAgroRange()
    {
        return Vector3.Distance(target.transform.position, transform.position) < visionRange;
    }

    private bool IsTargetSameHeight()
    {
        float deltaY = Mathf.Abs(target.transform.position.y - transform.position.y);
    
        return deltaY <= 1.2f;
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
