using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    //variables used by the player for movement and combat
    [Header("Health")]
    public int maxHealth;
    public int currentHealth;

    [Header("Attack")]
    public int attackDamage;
    public float baseAttackSpeed;
    public float currentAttackSpeed;

    [Header("Movement")]
    public float baseMoveSpeed;
    public float currentMoveSpeed;
    public int maxJumps;
}
