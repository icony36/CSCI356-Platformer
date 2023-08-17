using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Public References
    public PlayerState CurrentState { get; protected set; }
    public Animator Animator { get; private set; }
    public Combat Combat { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public Movement Movement { get; private set; }

    public PlayerData playerData; 
    public PlayerData initData;

    // Animation Params
    public const string ANIM_ATTACK = "Attack";
    public const string ANIM_ATTACK_INDEX = "AttackIndex";
    public const string ANIM_MOVE_SPEED = "MoveSpeed";
    public const string ANIM_JUMP = "IsJumping";
    public const string ANIM_FALL = "IsFalling";
    public const string ANIM_LAND = "IsLanded";
    public const string ANIM_DEAD = "Dead";
    public const string ANIM_HURT = "Hurt";
    public const string ANIM_SKILL = "Skill";
    public const string ANIM_ON_CLIMB = "IsOnClimb";
    public const string ANIM_CLIMB_SPEED = "ClimbSpeed";

    public enum PlayerState
    {
        Normal,
        Attacking,
        Casting,
        Dead
    }

    protected void Awake()
    {
        Animator = GetComponent<Animator>();
        Combat = GetComponent<Combat>();
        PlayerController = GetComponent<PlayerController>();
        Movement = GetComponent<Movement>();

        //ideally this initialization should be done in GameManager
        playerData.maxHealth = initData.maxHealth;
        playerData.attackDamage = initData.attackDamage;
        playerData.baseAttackSpeed = initData.baseAttackSpeed;
        playerData.baseMoveSpeed = initData.baseMoveSpeed;
        playerData.maxJumps= initData.maxJumps;
    }

    public void SwitchPlayerState(PlayerState newState)
    {
        // exiting current state 
        switch (CurrentState)
        {
            case PlayerState.Normal:
                break;
            case PlayerState.Attacking:
                Combat.AttackHitbox.DisableHitBox();
                Animator.ResetTrigger(ANIM_ATTACK);
                break;
            case PlayerState.Casting:
                Animator.ResetTrigger(ANIM_SKILL);
                break;
            case PlayerState.Dead:
                Animator.ResetTrigger(ANIM_DEAD);
                break;
            default:
                break;
        }

        CurrentState = newState;

        // entering new state 
        switch (newState)
        {
            case PlayerState.Normal:
                EnableAllActions();
                break;
            case PlayerState.Attacking:
                DisableAllActions();
                break;
            case PlayerState.Casting:
                Movement.CanClimb = false;
                Combat.CanAttack = false;
                Combat.CanSkill = false;
                break;
            case PlayerState.Dead:
                Animator.SetTrigger(ANIM_DEAD);
                DisableAllActions();
                break;
            default:
                break;
        }        
    }

    private void EnableAllActions()
    {
        Movement.CanMove = true;
        Movement.CanClimb = true;
        Combat.CanAttack = true;
        Combat.CanSkill = true;
    }

    private void DisableAllActions()
    {
        Movement.CanMove = false;
        Movement.CanClimb = false;
        Combat.CanAttack = false;
        Combat.CanSkill = false;
    }

    public void ResetAllAnimTriggers()
    {
        foreach (AnimatorControllerParameter param in Animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                Animator.ResetTrigger(param.name);
            }
        }
    }

    public void PlayAnimMove(float normalizeMoveSpeed, float baseMoveSpeed, float currentMoveSpeed)
    {
        Animator.SetFloat(ANIM_MOVE_SPEED, normalizeMoveSpeed, 0.1f, Time.deltaTime);
    }

    public void PlayAnimJump(bool isJump)
    {
        Animator.SetBool(ANIM_JUMP, isJump);
    }

    public void PlayAnimFall(bool isFall)
    {
        Animator.SetBool(ANIM_FALL, isFall);
    }

    public void PlayAnimLand(bool isLand)
    {
        Animator.SetBool(ANIM_LAND, isLand);
    }

    public void PlayAnimAttack(int animIndex, float baseAttackSpeed, float currentAttackSpeed)
    {
        Animator.SetInteger(ANIM_ATTACK_INDEX, animIndex);
        Animator.SetTrigger(ANIM_ATTACK);
    }

    public void PlayAnimHurt()
    {
        Animator.SetTrigger(ANIM_HURT);
    }

    public void PlayAnimDead()
    {
        Animator.SetTrigger(ANIM_DEAD);
    }

    public void PlayAnimSkill()
    {
        Animator.SetTrigger(ANIM_SKILL);
    }

    public void PlayAnimOnClimb(bool onClimb)
    {
        Animator.SetBool(ANIM_ON_CLIMB, onClimb);
    }

    public void PlayAnimClimb(float climbSpeed)
    {
        Animator.SetFloat(ANIM_CLIMB_SPEED, climbSpeed);
    }
}

