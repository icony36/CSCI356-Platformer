using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] private float attackInterval = 5f;

    [field: Header("Skill")]
    [SerializeField] private float skillCooldown = 5f;
    [SerializeField] private bool skillUsed = false;
    [SerializeField] private GameObject skillPrefab;

    [Header("Floating Damage Text")]
    [SerializeField] private GameObject DamageTextPrefab;

    // References
    private Player player;
    private PlayerData playerData;

    // Local Variables
    private int currentAttackIndex = 0;
    private float currentAttackTimer = 0f;


    private void Start()
    {
        player = GetComponent<Player>();

        playerData = player.playerData;
        playerData.currentHealth = playerData.maxHealth;
        playerData.currentAttackSpeed = playerData.baseAttackSpeed;
    }

    private void Update()
    {
        currentAttackTimer += Time.deltaTime;
        if (currentAttackTimer >= attackInterval)
        {
            currentAttackTimer = 0f;
            currentAttackIndex = 0;
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

        if (currentAttackIndex >= 3)
        {
            currentAttackIndex = 0;
        }
        player.SwitchPlayerState(Player.PlayerState.Attacking);
        player.PlayAnimAttack(currentAttackIndex, playerData.baseAttackSpeed, playerData.currentAttackSpeed);
        currentAttackIndex++;
    }

    public override void UseSkill()
    {
        if (!CanSkill || skillUsed) { return; }

        player.SwitchPlayerState(Player.PlayerState.Casting);
        player.PlayAnimSkill();

        GameObject skill = Instantiate(skillPrefab);
        skill.transform.position = transform.position;
        skill.transform.rotation = transform.rotation;

        skillUsed = true;

        StartCoroutine(SkillCooldown());
    }

    IEnumerator SkillCooldown()
    {
        float startTime = 0f;

        while (startTime < skillCooldown)
        {
            startTime += Time.deltaTime;

            yield return null;
        }

        skillUsed = false;
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
        // play vfx

        CheckIsDead();
    }

    // Animation Event
    public void AnimEvents_Hit()
    {
        AttackHitbox.EnableHitBox(playerData.attackDamage);

        // play sfx
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
