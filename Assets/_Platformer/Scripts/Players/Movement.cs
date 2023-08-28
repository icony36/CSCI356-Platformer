using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float turnSpeed = 25f;
    [SerializeField] private Vector3 moveVec = Vector3.zero;

    [Header("Jump")]
    [SerializeField] private int jumpCounter = 0;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float terminalVelocity = -20.0f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Dash")]
    [SerializeField] private bool isDashing = false;
    [SerializeField] private bool CanDash = true;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float dashCooldown = 3f;

    [Header("Slide")]
    [SerializeField] private Vector3 slideDirection = Vector3.zero;
    [SerializeField] private bool isSliding = false;
    [SerializeField] private bool onIceSurface = false;
    [SerializeField] private float slideStartAccel = 3f;

    [Header("Climb")]
    [SerializeField] private float climbSpeed = 3f;

    [Header("States")]
    // Public Variables
    public bool CanMove = true;
    public bool CanClimb = true;
    public bool facingRight;

    // Jump
    private float yVelocity = 0f;
    private bool isJumping = false;

    // Climb
    public bool isNearLadder = false;
    private bool isClimbing = false;    
    private Transform ladderTransform;

    // References
    private Player player;
    private PlayerData playerData; 
    private CharacterController characterController;
    private TrailRenderer motionTrail;
    private AudioManager audioManager;

    private float startingColliderRadius;
    private Vector3 knockBackImpact;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
        motionTrail = GetComponentInChildren<TrailRenderer>();
        motionTrail.emitting = false;

        playerData = player.playerData;
        playerData.currentAttackSpeed = playerData.baseAttackSpeed;
        playerData.currentMoveSpeed = playerData.baseMoveSpeed;
        startingColliderRadius = characterController.radius;
    }

    public void MovePlayer(float moveValue, bool shouldMove, bool shouldJump, bool shouldDash)
    {
        if (isClimbing) { return; }

        if (moveValue > 0)
        {
            facingRight = true;

        }
        else if(moveValue < 0)
        { 
            facingRight = false;
        }

        moveVec = new Vector3(0, 0, 0);

        // add yVeloctiy with gravity
        yVelocity += gravity * Time.deltaTime;

        // move
        if (CanMove)
        {
            // only move on x axis
            moveVec.x = moveValue;

            // prevent move faster when moving diagonally
            moveVec.Normalize();

            // rotate to movement direction
            if (moveVec != Vector3.zero) // prevent turning back to original rotation
            {
                if(moveVec.x > 0)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), Time.deltaTime * turnSpeed);
                else if(moveVec.x < 0)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), Time.deltaTime * turnSpeed);
            }

            // move animation
            player.PlayAnimMove(moveVec.magnitude, playerData.baseMoveSpeed, playerData.currentMoveSpeed);
        }

        if (characterController.isGrounded)
        {
            yVelocity = -0.5f; // set to -0.5f to prevent jiggle

            jumpCounter = 0;
            isJumping = false;
            //isFalling = false;
            player.PlayerCombat.CanSkill = true;
            motionTrail.emitting = false;

            player.PlayAnimLand(true);
            player.PlayAnimJump(false);
            player.PlayAnimFall(false);

        }
        else // falling
        {
            //isFalling = true;
            player.PlayerCombat.CanSkill = false;
            motionTrail.emitting = true;

            player.PlayAnimLand(false);

            if ((isJumping && yVelocity < 0) || yVelocity < -2)
            {
                player.PlayAnimFall(true);
            }
        }

        // jump
        if (CanMove && jumpCounter < playerData.maxJumps && shouldJump)
        {
            if (onIceSurface && isSliding)
            {
                StopCoroutine(Slide());
                slideDirection.x = 0;
                isSliding = false;
                onIceSurface = false;
            }
            Jump();
            
        }

        // dash
        if (CanDash && !isSliding && shouldDash && moveVec != Vector3.zero)
        {
            StartCoroutine(Dash(moveVec));
        }

        if (isDashing) { return; }

        // add movement speed before apply gravity
        moveVec *= playerData.currentMoveSpeed;

        // clamp yVeloctiy at terminal veloctiy
        if (yVelocity < terminalVelocity)
        {
            yVelocity = terminalVelocity;
        }

        // apply gravity
        moveVec.y = yVelocity;

        if (characterController.isGrounded && onIceSurface && !isSliding && shouldMove)
        {
            StartCoroutine(Slide());
        }
        else if (onIceSurface && isSliding && shouldMove)
        {
            if (moveVec != slideDirection)
            {
                slideDirection.x = moveVec.x;
            }
        }

        // knock back
        if (knockBackImpact.magnitude > 0.2f)
        {
            knockBackImpact *= Time.deltaTime * 20f;
            knockBackImpact.y = 0;

            moveVec = knockBackImpact;          
        }
        knockBackImpact = Vector3.Lerp(knockBackImpact, Vector3.zero, Time.deltaTime * 5);

        // pass the movement to the character controller
        if (!onIceSurface)
        {
            characterController.Move(moveVec * Time.deltaTime);
        }
        else if (onIceSurface && !isSliding)
        {
            characterController.Move(moveVec * Time.deltaTime);
        }
    }

    private void Jump()
    {
        yVelocity = jumpSpeed;

        player.PlayAnimJump(true);

        // play sfx
        audioManager?.PlaySFX("Jump");

        isJumping = true;

        jumpCounter++;
    }

    IEnumerator Dash(Vector3 movement)
    {
        // play sfx
        audioManager?.PlaySFX("Dash");

        motionTrail.emitting = true;
        CanDash = false;
        isDashing = true;
        CanMove = false;

        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            characterController.Move(movement * dashSpeed * Time.deltaTime);

            yield return null;
        }

        motionTrail.emitting = false;
        CanMove = true;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        CanDash = true;
    }

    public void Climb(float climbValue, bool shouldJump)
    {
        if (!CanClimb) { return; }     

        if (shouldJump)
        {
            CancelClimb();
        }

        if (isNearLadder)
        {
            if (!isClimbing && climbValue > 0)
            {
                isClimbing = true;
                player.DisableCombatActions();
                player.PlayAnimOnClimb(true);
                characterController.radius = 0.2f;

                characterController.enabled = false;
                transform.position = ladderTransform.position;
                transform.rotation = ladderTransform.rotation;
                characterController.enabled = true;
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

    public void SetIsNearLadder(Transform ladderTransform)
    {
        isNearLadder = true;
        this.ladderTransform = ladderTransform;
    }

    public void ExitClimb(Vector3 exitPosition)
    {
        ladderTransform = null;

        if (isClimbing)
        {
            characterController.enabled = false;
            transform.position = exitPosition;
            characterController.enabled = true;

            CancelClimb();
        }
    }

    private void CancelClimb()
    {
        isNearLadder = false;
        isClimbing = false;
        player.EnableCombatActions();
        player.PlayAnimOnClimb(false);
        characterController.radius = startingColliderRadius;
    }

    IEnumerator Slide()
    {
        slideDirection = moveVec;

        isSliding = true;

        while (isSliding)
        {
            characterController.Move(slideDirection * Mathf.Lerp(slideStartAccel, 0f, -0.5f) * Time.deltaTime);
            yield return null;
        }

        isSliding = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("IceSurface"))
        {
            onIceSurface = true;

            if(moveVec.x != 0)
            {
                StartCoroutine(Slide());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("IceSurface"))
        {
            onIceSurface = false;
            isSliding = false;
        }
    }

    public void EnableCharacterController(bool shouldEnable)
    {
        characterController.enabled = shouldEnable;
    }

    public void AddKnockBack(Vector3 damageSource, float force)
    {
        Vector3 impactDirection = transform.position - damageSource;
        impactDirection.Normalize();
        impactDirection.y = 0;

        knockBackImpact = impactDirection * force;
    }
}

