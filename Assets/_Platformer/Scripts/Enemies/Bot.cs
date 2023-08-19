using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Bot : MonoBehaviour
{
    [Header("Patrolling")]
    [SerializeField] private Transform patrolEndPoint;
    [SerializeField] private float visionRange = 10f;

    [Header("Chasing")]
    [Tooltip("Stopping distance for attack.")]
    [SerializeField] private float attackRange = 1.6f;
    [Tooltip("Speed of attack animation.")]
    [SerializeField] private float attackRate = 1f;
    [Tooltip("In seconds.")]
    [SerializeField] private float attackCooldown;

    [Header("Movement")]
    [SerializeField] private float speed = 10;

    // References  
    private Animator animator;
    private Transform target;
    private Combat combat;

    // Animation Params
    private const string ANIM_ATTACK = "Attack";
    private const string ANIM_ATTACK_SPEED = "AttackSpeed";
    private const string ANIM_SPEED = "Speed";
    private const string ANIM_HURT = "Hurt";
    private const string ANIM_DEAD = "Dead";

    // State Machine
    public enum BotState
    {
        Patrolling,
        Chasing,
        Dead
    }

    // Variables
    private string targetTag;
    private BotState currentState;
    private Vector3 startingPosition;
    private Vector3 endingPosition;
    private float lastAttackTime = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        combat = GetComponent<Combat>();
        
        targetTag = combat?.TargetTag;

        target = GameObject.FindWithTag(targetTag).transform;

        startingPosition = transform.position;
        endingPosition = patrolEndPoint.position;

        currentState = BotState.Patrolling;
    }

    private void Update()
    {
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
                break;
            default:
                break;
        }

        currentState = newState;

        switch (currentState)
        {
            case BotState.Patrolling:
                break;
            case BotState.Chasing:
                break;
            case BotState.Dead:
                animator.SetTrigger(ANIM_DEAD);
                break;
            default:
                break;
        }        
    }

    private void Patrolling()
    {
        if (target == null) { return; }

        // chase target if target is within sight and agro range
        if (CanSeeTarget())
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

            MoveToTarget(endingPosition);
        }
    }

    private void Chasing()
    {
        if (target == null) { return; }

        if (Vector3.Distance(target.position, transform.position) < visionRange)
        {
            if (Vector3.Distance(target.position, transform.position) < attackRange)
            {
                PlayAnimStop();

                if(Time.time - lastAttackTime >= attackCooldown)
                {
                    RotateToTarget();
                    combat.Attack();
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                MoveToTarget(target.position);
            }
        }
        else
        {
            // switch back to patrolling after 1 second
            StartCoroutine(DelaySwitchToPatrolling(1));
        }
    }

    private void MoveToTarget(Vector3 targetPosition)
    {
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

    private IEnumerator DelaySwitchToPatrolling(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        currentState = BotState.Patrolling;
    }

    private bool CanSeeTarget()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 halfExtents = transform.localScale / 2;
        Vector3 direction = transform.forward;
        Quaternion orientation = transform.rotation;
        float maxDistance = visionRange;

        RaycastHit hitInfo;

        DebugTools.DrawBoxCastBox(origin, halfExtents, direction, orientation, maxDistance, Color.blue);

        if (Physics.BoxCast(origin, halfExtents,direction, out hitInfo, orientation, maxDistance))
        {
            if (hitInfo.transform.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    public void RotateToTarget()
    {
        if (currentState != BotState.Dead)
        {
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z), Vector3.up);
        }
    }


    public void PlayAnimAttack()
    {
        animator.SetTrigger(ANIM_ATTACK);
        animator.SetFloat(ANIM_ATTACK_SPEED, attackRate);
    }

    public void PlayAnimStop()
    {
        animator.SetFloat(ANIM_SPEED, 0f);
    }

    public void PlayAnimHurt()
    {
        animator.SetTrigger(ANIM_HURT);
    }

    // Animation Event
    public void AnimEvents_DeadEnd()
    {
        gameObject.SetActive(false);
    }



}
