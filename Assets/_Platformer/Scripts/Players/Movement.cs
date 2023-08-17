using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float turnSpeed = 25f;

    [Header("Jump")]
    [SerializeField] private int jumpCounter = 0;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float terminalVelocity = -20.0f;
    [SerializeField] private float gravity = -9.81f;
    [Tooltip("In seconds")]
    [SerializeField] private float fallingDeathThreshold = 3.0f;

    [Header("Climb")]
    [SerializeField] private float climbSpeed = 3f;

    // Public Variables
    public bool CanMove = true;
    public bool CanClimb = true;
    public bool IsNearLadder = false;

    // Jump
    private float yVelocity = 0f;
    private bool isJumping = false;
    private bool isDashing = false;
    private bool isFalling = false;
    private float isFallingTimer = 0f;

    // Climb
    private bool isClimbing = false;

    // References
    private Player player;
    private PlayerData playerData;
    private CharacterController characterController;

    private float startingColliderRadius;
    private Vector3 climbEndPosition;

    private void Start()
    {
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();

        playerData = player.playerData;
        playerData.currentHealth = playerData.maxHealth;
        playerData.currentAttackSpeed = playerData.baseAttackSpeed;
        playerData.currentMoveSpeed = playerData.baseMoveSpeed;
        startingColliderRadius = characterController.radius;
    }

    private void Update()
    {
        if (isFalling && !isClimbing)
        {
            isFallingTimer += Time.deltaTime;

            if (isFallingTimer >= fallingDeathThreshold && player.CurrentState != Player.PlayerState.Dead)
            {
                player.PlayerCombat.InstantKill();
            }
        }
        else
        {
            isFallingTimer = 0f;
        }
    }

    public void MovePlayer(float moveValue, bool shouldJump, bool shouldDash)
    {
        if (isClimbing) { return; }

        Vector3 movement = new Vector3(0, 0, 0);

        // add yVeloctiy with gravity
        yVelocity += gravity * Time.deltaTime;

        // move
        if (CanMove)
        {
            // only move on x axis
            movement.x = moveValue;

            // prevent move faster when moving diagonally
            movement.Normalize();

            // rotate to movement direction
            if (movement != Vector3.zero) // prevent turning back to original rotation
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * turnSpeed);
            }

            // move animation
            player.PlayAnimMove(movement.magnitude, playerData.baseMoveSpeed, playerData.currentMoveSpeed);
        }

        if (characterController.isGrounded)
        {
            yVelocity = -0.5f; // set to -0.5f to prevent jiggle

            jumpCounter = 0;
            isJumping = false;
            isFalling = false;

            player.PlayAnimLand(true);
            player.PlayAnimJump(false);
            player.PlayAnimFall(false);

            // jump
            if (CanMove && shouldJump)
            {
                Jump();
            }

            // dash
            if(CanMove && shouldDash)
            {
                Dash();
            }
        }
        else // falling
        {
            isFalling = true;

            if (jumpCounter < playerData.maxJumps && shouldJump)
            {
                Jump();
            }

            if (shouldDash)
            {
                Dash();
            }

            player.PlayAnimLand(false);

            if ((isJumping && yVelocity < 0) || yVelocity < -2)
            {
                player.PlayAnimFall(true);
            }
        }

        // add movement speed before apply gravity
        movement *= playerData.currentMoveSpeed;

        // clamp yVeloctiy at terminal veloctiy
        if (yVelocity < terminalVelocity)
        {
            yVelocity = terminalVelocity;
        }

        // apply gravity
        movement.y = yVelocity;

        // pass the movement to the character controller
        characterController.Move(movement * Time.deltaTime);
    }

    //public void SetMoveSpeed(float speed)
    //{
    //    playerData.currentMoveSpeed = Mathf.Clamp(speed, 0f, speed);
    //}
    private void Jump()
    {
        yVelocity = jumpSpeed;

        player.PlayAnimJump(true);
        // play sfx
        // play vfx

        isJumping = true;

        jumpCounter++;
    }

    private void Dash()
    {

    }

    public void Climb(float climbValue, bool shouldJump)
    {
        if (!CanClimb) { return; }

        if (shouldJump)
        {
            CancelClimb();
        }

        if (IsNearLadder)
        {
            if (!isClimbing && climbValue > 0)
            {
                isClimbing = true;
                player.PlayAnimOnClimb(true);
                characterController.radius = 0.2f;
                characterController.Move(Vector3.right);
            }
            else if (isClimbing)
            {                               
                Vector3 movement = new Vector3(0, climbValue, 0);

                player.PlayAnimClimb(climbValue);

                characterController.Move(movement * Time.deltaTime * climbSpeed);
            }            
        }
        else
        {
            CancelClimb();
        }
    }

    public void ExitClimb(Vector3 exitPosition)
    {
        climbEndPosition = exitPosition;
        if (isClimbing)
        {
            characterController.enabled = false;
            transform.position = climbEndPosition;
            characterController.enabled = true;

            CancelClimb();
        }
    }

    private void CancelClimb()
    {
        IsNearLadder = false;
        isClimbing = false;
        player.PlayAnimOnClimb(false);
        characterController.radius = startingColliderRadius;
    }
}

