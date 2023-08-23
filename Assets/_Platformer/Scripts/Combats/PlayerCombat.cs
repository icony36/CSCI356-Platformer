using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class PlayerCombat : Combat
{
    [field: SerializeField] public Color TagColor { get; private set; } = new Color(0, 255, 0);

    [field: Header("Normal Attack")]
    //[field: SerializeField] public int AttackDamage { get; private set; } = 10;
    //[SerializeField] private float baseAttackSpeed = 1f;
    //[Tooltip("Only shown for testing purpose")]
    //[SerializeField] private float currentAttackSpeed;

    [field: Header("Ranged Attack")]
    [SerializeField] private GameObject directionIndicator;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private bool aimingMode = false;
    [SerializeField] private float attackInterval = 1f;

    [field: Header("Skill")]
    [SerializeField] private float skillCooldown = 5f;
    [SerializeField] private bool skillUsed = false;
    [SerializeField] private GameObject skillPrefab;

    [Header("Floating Damage Text")]
    [SerializeField] private GameObject DamageTextPrefab;

    // References
    private Player player;
    private PlayerData playerData;
    private BuffIndicator buffIndicator;
    private AudioManager audioManager;

    // Local Variables
    private int currentAttackIndex = 0;
    private float currentAttackTimer = 0f;
    private bool canFire = true;

    private void Start()
    {
        player = GetComponent<Player>();
        buffIndicator = GameObject.FindGameObjectWithTag("UICanvas")?.GetComponent<BuffIndicator>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager")?.GetComponent<AudioManager>();

        playerData = player.playerData;
        playerData.currentHealth = playerData.maxHealth;
        playerData.currentAttackSpeed = playerData.baseAttackSpeed;
    }

    private void Update()
    {
        if(!canFire)
            currentAttackTimer += Time.deltaTime;

        if (currentAttackTimer >= attackInterval)
        {
            currentAttackTimer = 0f;
            canFire = true;
        }

        if (aimingMode)
        {
            float rot = 0;
            rot -= Mouse.current.delta.y.ReadValue() * Time.deltaTime * 25.0f;

            if (!(directionIndicator.transform.localEulerAngles.x + rot >= 45 && directionIndicator.transform.localEulerAngles.x + rot <= 315))
            {
                directionIndicator.transform.localEulerAngles += new Vector3(rot, 0, 0);
            }      
        }
    }

    public override void InstantKill()
    {
        playerData.currentHealth = 0;

        CheckIsDead();
    }

    public void SetAttackSpeed(float speed)
    {
        playerData.currentAttackSpeed = Mathf.Clamp(speed, 0f, 2 * playerData.baseAttackSpeed);
    }

    public override void Attack()
    {
        if (!CanAttack) { return; }

        if(!aimingMode)
        {
            if (currentAttackIndex >= 3)
            {
                currentAttackIndex = 0;
            }
            player.SwitchPlayerState(Player.PlayerState.Attacking);
            player.PlayAnimAttack(currentAttackIndex, playerData.baseAttackSpeed, playerData.currentAttackSpeed);
            currentAttackIndex++;
        }
        else
        {
            if(!canFire) { return; };

            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = directionIndicator.transform.GetChild(0).position;
            projectile.transform.eulerAngles = new Vector3(directionIndicator.transform.eulerAngles.x, transform.eulerAngles.y, 0);

            canFire = false;
        }
    }

    public override void UseSkill()
    {
        if (!CanSkill || skillUsed) { return; }

        player.SwitchPlayerState(Player.PlayerState.Casting);
        player.PlayAnimSkill();

        // play sfx
        audioManager?.PlaySFX(7);

        GameObject skill = Instantiate(skillPrefab);
        skill.transform.position = transform.position;
        skill.transform.rotation = transform.rotation;

        skillUsed = true;
        

        StartCoroutine(SkillCooldown());
    }

    IEnumerator SkillCooldown()
    {
        float startTime = 0f;
        buffIndicator.SetCoolDownRotationFill(0f);
        buffIndicator.SetCoolDownOpacity(0.5f);

        while (startTime < skillCooldown)
        {
            startTime += Time.deltaTime;

            buffIndicator.SetCoolDownRotationFill(startTime/skillCooldown);

            yield return null;
        }

        skillUsed = false;
        buffIndicator.SetCoolDownRotationFill(1f);
        buffIndicator.SetCoolDownOpacity(1f);
    }

    public void ToggleAttackMode()
    {
        aimingMode = !aimingMode;
        directionIndicator.SetActive(aimingMode);
    }    

    public override void CheckIsDead()
    {
        if (playerData.currentHealth <= 0)
        {
            player?.SwitchPlayerState(Player.PlayerState.Dead);
        }
    }
    public override void InflictDamage(float damageToInflict)
    {
        if (isInvincible) { return; }

        playerData.currentHealth -= (int)Mathf.Clamp(damageToInflict, 0, damageToInflict);

        player.PlayAnimHurt();

        // instantiate floating damage
        DamageIndicator indicator = Instantiate(DamageTextPrefab, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        indicator.SetDamageText(damageToInflict);

        // play sfx
        audioManager?.PlaySFX(3);
        // play vfx

        CheckIsDead();
    }

    // Animation Event
    public void AnimEvents_Hit()
    {
        AttackHitbox.EnableHitBox(playerData.attackDamage);

        // play sfx
        audioManager?.PlaySFX(1);
        // play vfx
    }

    // Animation Event
    public void AnimEvents_Hit1()
    {
        AttackHitbox.EnableHitBox(playerData.attackDamage);

        // play sfx
        AudioManager.instance.PlaySFX(2);
        // play vfx
    }

    // Animation Event
    public void AnimEvents_Hit2()
    {
        AttackHitbox.EnableHitBox(playerData.attackDamage);

        // play sfx
        AudioManager.instance.PlaySFX(1);
        // play vfx
    }

    // Animation Event
    public void AnimEvents_Hit3()
    {
        AttackHitbox.EnableHitBox(playerData.attackDamage);

        // play sfx
        AudioManager.instance.PlaySFX(2);
        // play vfx
    }

    // Animation Event
    public void AnimEvents_HitEnd()
    {
        AttackHitbox.DisableHitBox();
    }

    // Animation Event
    public void AnimEvents_End()
    {
        player.SwitchPlayerState(Player.PlayerState.Normal);
    }
}
