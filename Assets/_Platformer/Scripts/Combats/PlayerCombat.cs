using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Combat
{
    [field: SerializeField] public Color TagColor { get; private set; } = new Color(0, 255, 0);

    [Header("Knock Back")]
    [SerializeField] private float knockBackImpact = 10f;

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
    private ToggleIndicator toggleIndicator;

    // Local Variables
    private int currentAttackIndex = 0;
    private float currentAttackTimer = 0f;
    private bool canFire = true;

    private void Start()
    {
        player = GetComponent<Player>();
        buffIndicator = GameMenu.Instance?.GetComponent<BuffIndicator>();
        toggleIndicator = GameMenu.Instance?.GetComponent<ToggleIndicator>();

        playerData = player.playerData;
        
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
    }

    public void Aiming(float moveValueY)
    {
        if (!aimingMode) { return; }

        if (player.Movement.facingRight)
            transform.eulerAngles = new Vector3(0, 90, 0);
        else
            transform.eulerAngles = new Vector3(0, -90, 0);

        float rot = 0;
        rot -= moveValueY * Time.deltaTime * 80f;

        if (!(directionIndicator.transform.localEulerAngles.x + rot >= 45 && directionIndicator.transform.localEulerAngles.x + rot <= 315))
        {
            directionIndicator.transform.localEulerAngles += new Vector3(rot, 0, 0);
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

            player.PlayAnimRangeAttack();

            audioManager.PlaySFX("Attack2");

            canFire = false;
        }
    }

    public void UseSkill()
    {
        if (!CanSkill || skillUsed) { return; }

        player.SwitchPlayerState(Player.PlayerState.Casting);
        player.PlayAnimSkill();
    }

    private void SpawnSkill()
    {
        // play sfx
        audioManager?.PlaySFX("PlayerSkill");

        GameObject skill = Instantiate(skillPrefab);
        skill.transform.position = transform.position;
        skill.transform.rotation = Quaternion.LookRotation(new Vector3(transform.forward.x, 0f, 0f));
        

        skillUsed = true;

        StartCoroutine(SkillCooldown());
    }

    IEnumerator SkillCooldown()
    {
        float startTime = 0f;
        buffIndicator.SetCDRotationFill(1f);

        while (startTime < skillCooldown)
        {
            startTime += Time.deltaTime;

            buffIndicator.SetCDRotationFill((skillCooldown - startTime) / skillCooldown);

            yield return null;
        }

        skillUsed = false;
        buffIndicator.SetCDRotationFill(0f);
    }

    public void ToggleAttackMode()
    {
        aimingMode = !aimingMode;
        directionIndicator.SetActive(aimingMode);
        toggleIndicator.ToggleAttackType(aimingMode);
    }    

    public override void CheckIsDead()
    {
        if (playerData.currentHealth <= 0)
        {
            player.SwitchPlayerState(Player.PlayerState.Dead);
        }
    }
    public override void InflictDamage(float damageToInflict, Vector3 damageSource)
    {
        if (isInvincible) { return; }

        int finalDamage = Mathf.CeilToInt(damageToInflict) + Mathf.FloorToInt(Random.Range(-3f, 3f));

        playerData.currentHealth -= Mathf.Clamp(finalDamage, 0, finalDamage);

        player.PlayAnimHurt();

        player.Movement.AddKnockBack(damageSource, knockBackImpact);

        // instantiate floating damage
        DamageIndicator indicator = Instantiate(DamageTextPrefab, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        indicator.SetDamageText(finalDamage);

        // play sfx
        audioManager.PlaySFX("Hurt");

        CheckIsDead();
    }

    public void GodMode()
    {
        isInvincible = !isInvincible;
    }

    // Animation Event
    public void AnimEvents_Hit()
    {
        AttackHitbox.EnableHitBox(playerData.attackDamage);

        // play sfx
        audioManager?.PlaySFX("Attack1");
    }

    // Animation Event
    public void AnimEvents_Hit2()
    {
        AttackHitbox.EnableHitBox(playerData.attackDamage);

        // play sfx
        audioManager?.PlaySFX("Attack2");
    }

    // Animation Event
    public void AnimEvents_HitEnd()
    {
        AttackHitbox.DisableHitBox();
    }

    // Animation Event
    public void AnimEvents_Cast()
    {
        SpawnSkill();
    }

    // Animation Event
    public void AnimEvents_End()
    {
        player.SwitchPlayerState(Player.PlayerState.Normal);
    }
}
