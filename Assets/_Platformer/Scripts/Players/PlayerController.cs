using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // References
    private Player player;

    // Input Action
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction attackAction;
    private InputAction skillAction;
    private InputAction rangeToggle;

    private void Start()
    {
        player = GetComponent<Player>();      

        moveAction = player.PlayerInput?.actions["Move"];
        jumpAction = player.PlayerInput?.actions["Jump"];
        dashAction = player.PlayerInput?.actions["Dash"];
        attackAction = player.PlayerInput?.actions["Attack"];
        skillAction = player.PlayerInput?.actions["Skill"];
        rangeToggle = player.PlayerInput?.actions["Toggle"];
    }

    private void Update()
    {
        HandleMoveInput();

        HandleAimingInput();

        HandleAttackInput();

        HandleSkillInput();
    }

    private void HandleMoveInput()
    {
        if (moveAction == null) { return; }
        if (jumpAction == null) { return; }

        if (player.Animator.GetCurrentAnimatorStateInfo(0).IsName("Skill") && player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return;

        // back to normal state if can
        if (moveAction.inProgress)
        {
            if (player.CurrentState != Player.PlayerState.Normal && player.CurrentState != Player.PlayerState.Dead)
            {
                if (player.Movement.CanMove)
                {
                    player.SwitchPlayerState(Player.PlayerState.Normal);
                }
            }
        }

        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        // move
        player.Movement.MovePlayer(moveValue.x, moveAction.triggered, jumpAction.triggered, dashAction.triggered);
        // climb
        player.Movement.Climb(moveValue.y, jumpAction.triggered);
    }

    private void HandleAttackInput()
    {
        if (attackAction == null) { return; }

        if (attackAction.triggered)
        {
            player.PlayerCombat.Attack();
        }

        if(rangeToggle.triggered)
        {
            player.PlayerCombat.ToggleAttackMode();
        }
    }

    private void HandleSkillInput()
    {
        if (skillAction == null) { return; }

        if (skillAction.triggered)
        {
            player.PlayerCombat.UseSkill();
        }
    }

    private void HandleAimingInput()
    {
        if (moveAction == null) { return; }

        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        player.PlayerCombat.Aiming(moveValue.y);
    }
}

