using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Bot : MonoBehaviour
{
    public int ID;
    public EnemyVFXManager EnemyVFXManager { get; protected set; }

    [Header("Patrolling")]
    [SerializeField] protected bool shouldPatrol = true;
    [SerializeField] protected float visionRange = 10f;
    [SerializeField] protected Transform patrolEndPoint;

    [Header("Attacking")]
    [Tooltip("Stopping distance for attack.")]
    [SerializeField] protected float attackRange = 1.6f;
    [Tooltip("Speed of attack animation.")]
    [SerializeField] protected float attackRate = 1f;
    [Tooltip("In seconds.")]
    [SerializeField] protected float attackCooldown;

    [Header("Movement")]
    [SerializeField] protected float speed = 10;

    // References  
    protected Animator animator;
    protected GameObject target;
    protected Combat combat;
    protected Collider coliider;

    // Animation Params
    protected const string ANIM_ATTACK = "Attack";
    protected const string ANIM_ATTACK_SPEED = "AttackSpeed";
    protected const string ANIM_SPEED = "Speed";
    protected const string ANIM_HURT = "Hurt";
    protected const string ANIM_DEAD = "Dead";

    // State Machine
    public enum BotState
    {
        Patrolling,
        Chasing,
        Dead
    }

    // Variables
    protected string targetTag;
    protected BotState currentState;
    protected Vector3 startingPosition;
    protected Vector3 endingPosition;
    protected Quaternion startingRotation;
    protected float lastAttackTime = 0f;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        combat = GetComponent<Combat>();
        coliider = GetComponent<Collider>();
        EnemyVFXManager = GetComponent<EnemyVFXManager>();

        targetTag = combat?.TargetTag;

        target = GameObject.FindGameObjectWithTag(targetTag);

        startingPosition = transform.position;
        startingRotation = transform.rotation;
        endingPosition = patrolEndPoint.position;

        currentState = BotState.Patrolling;
    }

    protected virtual void Update()
    {             
        if(CheckIsTargetDead())
        {
            PlayAnimIdle();
            
            return;
        }
        
        switch (currentState)
        {
            case BotState.Patrolling:
                Patrolling();
                break;
            case BotState.Chasing:
                Chasing();
                break;
            case BotState.Dead:
                return;
            default:
                break;
        }
    }

    public void SwitchBotState(BotState newState)
    {
        // exiting current state
        switch (currentState)
        {
            case BotState.Patrolling:               
                break;
            case BotState.Chasing:
                animator.ResetTrigger(ANIM_ATTACK);
                break;
            case BotState.Dead:
                animator.ResetTrigger(ANIM_DEAD);
                coliider.enabled = true;
                break;
            default:
                break;
        }

        currentState = newState;

        // entering new state
        switch (currentState)
        {
            case BotState.Patrolling:            
                break;
            case BotState.Chasing:
                break;
            case BotState.Dead:
                animator.SetTrigger(ANIM_DEAD);
                coliider.enabled = false;
                break;
            default:
                break;
        }        
    }

    protected virtual void Patrolling()
    {
        if (target == null) { return; }        

        // chase target if target is within sight and agro range
        if(CanSeeTarget())
        {
            currentState = BotState.Chasing;
        }
        else
        {            
            if (Vector3.Distance(endingPosition, transform.position) < 0.5f)
            {
                // swap ending and starting position
                Vector3 temp = endingPosition;
                endingPosition = startingPosition;
                startingPosition = temp;
            }

            if (shouldPatrol)
            {
                MoveToTarget(endingPosition);   
            }
            else
            {
                transform.rotation = startingRotation;
                PlayAnimIdle();
            }
        }
    }

    protected virtual void Chasing()
    {
        if (target == null) { return; }

        if (Vector3.Distance(target.transform.position, transform.position) < visionRange)
        {
            if (Vector3.Distance(target.transform.position, transform.position) < attackRange)
            {
                PlayAnimIdle();

                if(Time.time - lastAttackTime >= attackCooldown)
                {
                    RotateToTarget();
                    combat.Attack();
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                if (shouldPatrol)
                {
                    MoveToTarget(target.transform.position);
                }
                else
                {
                    PlayAnimIdle();
                }
            }
        }
        else
        {
            // switch back to patrolling after 1 second
            StartCoroutine(DelaySwitchToPatrolling(1));
        }
    }

    protected virtual void MoveToTarget(Vector3 targetPosition)
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
        float maxX = Mathf.Max(startingPosition.x, endingPosition.x);
        float minX = Mathf.Min(startingPosition.x, endingPosition.x);
        if (transform.position.x <= maxX && transform.position.x >= minX)
        {
            // move forward
            transform.Translate(0, 0, 0.2f * Time.deltaTime * speed);
        }
        else // relocate to nearest waypoint if out of boundary
        {
            float distFromStart = Vector3.Distance(transform.position, startingPosition);
            float distFromEnd = Vector3.Distance(transform.position, endingPosition);

            Vector3 nearestPoint = distFromStart < distFromEnd ? startingPosition : endingPosition;

            transform.position = nearestPoint;
        }

        animator.SetFloat(ANIM_SPEED, 1, 0.1f, Time.deltaTime);
    }

    protected void RotateToTarget()
    {
        if (currentState != BotState.Dead)
        {
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), Vector3.up);
        }
    }


    protected IEnumerator DelaySwitchToPatrolling(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        SwitchBotState(BotState.Patrolling);
    }

    protected bool CanSeeTarget()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Vector3 halfExtents = transform.localScale / 2;
        Vector3 direction = transform.forward;
        Quaternion orientation = transform.rotation;
        float maxDistance = visionRange;

        DebugTools.DrawBoxCastBox(origin, halfExtents, direction, orientation, maxDistance, Color.blue);

        //RaycastHit hitInfo;

        //if (Physics.BoxCast(origin, halfExtents, direction, out hitInfo, orientation, maxDistance))
        //{
        //    if (hitInfo.transform.CompareTag(targetTag))
        //    {
        //        return true;
        //    }
        //}

        float dot = Vector3.Dot(transform.forward, (target.transform.position - transform.position).normalized);
        float distance = Vector3.Distance(target.transform.position, transform.position);

        if (dot > 0.99f && distance <= visionRange) 
        { 
            return true; 
        }

        return false;
    }

    protected bool CheckIsTargetDead()
    {
        if (targetTag == "Player")
        {
            Player player = target.GetComponent<Player>();
            if (player.CurrentState == Player.PlayerState.Dead)
            {
                return true;
            }
        }

        return false;
    }

    public virtual void PlayAnimAttack()
    {
        animator.SetTrigger(ANIM_ATTACK);
        animator.SetFloat(ANIM_ATTACK_SPEED, attackRate);
    }

    public virtual void PlayAnimIdle()
    {
        animator.SetFloat(ANIM_SPEED, 0f);
    }

    public virtual void PlayAnimHurt()
    {
        animator.SetTrigger(ANIM_HURT);
    }

    // Animation Event
    public void AnimEvents_DeadEnd()
    {
        gameObject.SetActive(false);
    }
}
