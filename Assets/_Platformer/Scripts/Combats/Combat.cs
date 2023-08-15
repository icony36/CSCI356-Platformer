using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class Combat : MonoBehaviour
{

    [field: Header("Settings")]
    [field: SerializeField] public bool IsBot { get; private set; } = false;
    [field: SerializeField] public string TargetTag { get; private set; } = "Enemy";
    [field: Tooltip("Only for player.")]
    [field: SerializeField] public Color PlayerColor { get; private set; } = new Color(0, 255, 0);
    [field: Tooltip("Only for player.")]
    [field: SerializeField] public Color EnemyColor { get; private set; } = new Color(255, 0, 0);

    [field: Header("Health")]
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    [field: SerializeField] public int CurrentHealth { get; private set; }

    [Header("States")]
    [Tooltip("Only shown for testing purpose")]
    [SerializeField] private bool isInvincible;

    [field: Header("Normal Attack")]
    [field: SerializeField] public HitBox HitBox { get; private set; }
    [field: SerializeField] public int AttackDamage { get; private set; } = 10;
    [SerializeField] private float baseAttackSpeed = 1f;
    [Tooltip("Only shown for testing purpose")]
    [SerializeField] private float currentAttackSpeed;

    // Public Variables
    public bool CanAttack = true;
    public bool CanSkill = true;

    // References
    private Player player;
    private Bot bot;
    
    // Local Variables
    private int currentAttackIndex = 0;


    private void Start()
    {
        if (IsBot)
        {
            bot = GetComponent<Bot>();
        }
        else
        {
            player = GetComponent<Player>();
        }

        CurrentHealth = MaxHealth;
        currentAttackSpeed = baseAttackSpeed;
    }

    public void TakeDamage(int damage, Vector3 attackerPos = new Vector3())
    {
        if (isInvincible) { return; }

        CurrentHealth -= Mathf.Clamp(damage, 0, damage);

        player?.PlayAnimHurt(attackerPos);
        bot?.PlayAnimHurt(attackerPos);

        // play sfx
        // play vfx

        CheckIsDead();
    }

    public void CheckIsDead()
    {
        if (CurrentHealth <= 0)
        {
            player?.SwitchPlayerState(Player.PlayerState.Dead);
            bot?.SwitchBotState(Bot.BotState.Dead);
        }
    }

    public void SetAttackSpeed(float speed)
    {
        currentAttackSpeed = Mathf.Clamp(speed, 0f, 2 * baseAttackSpeed);
    }

    public void Attack()
    {
        if (!CanAttack) { return; }

        // for player
        if (currentAttackIndex >= 3)
        {
            currentAttackIndex = 0;
        }
        player?.SwitchPlayerState(Player.PlayerState.Attacking);
        player?.PlayAnimAttack(currentAttackIndex, baseAttackSpeed, currentAttackSpeed);
        currentAttackIndex++;

        // for bot
        bot?.PlayAnimAttack();
    }

    public void Skill()
    {
        if (!CanSkill) { return; }

        // for player
        player?.SwitchPlayerState(Player.PlayerState.Casting);
        player?.PlayAnimSkill();
    }

    // Animation Event
    public void AnimEvents_Hit()
    {
        HitBox.EnableHitBox();

        // play sfx
    }

    // Animation Event
    public void AnimEvents_HitEnd()
    {
        HitBox.DisableHitBox();
    }

    // Animation Event
    public void AnimEvents_End()
    {
        player?.SwitchPlayerState(Player.PlayerState.Normal);
        bot?.SwitchBotState(Bot.BotState.Patrolling);
    }
}
