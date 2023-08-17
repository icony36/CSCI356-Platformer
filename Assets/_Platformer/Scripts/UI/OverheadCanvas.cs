 using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LookAtCamera))]
public class OverheadCanvas : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject parent;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image healthBarOutline;
    [SerializeField] private Image healthFill;

    [Header("Settings")]
    [SerializeField] private float currentValue;
    [SerializeField] private float maxValue;
    [SerializeField] private float healthBarDropSpeed = 50f;

    private EnemyCombat enemyCombat;
    private Player player;
    private Color healthbarColor;

    private void Start()
    {
        parent = gameObject.transform.parent.gameObject;

        if (parent.tag == "Enemy")
        {
            enemyCombat = parent.GetComponent<EnemyCombat>();
            healthbarColor = enemyCombat.TagColor;
        }
        else if (parent.tag == "Player")
        {
            player = parent.GetComponent<Player>();
            healthbarColor = player.PlayerCombat.TagColor;
        }

        healthBarOutline.color = healthbarColor;
        healthFill.color = healthbarColor;
    }

    private void Update()
    {
        if (parent.tag == "Enemy")
        {
            currentValue = enemyCombat.CurrentHealth;
            maxValue = enemyCombat.MaxHealth;
        }
        else if (parent.tag == "Player")
        {
            currentValue = player.playerData.currentHealth;
            maxValue = player.playerData.maxHealth;
        }

        float percentage = (float)currentValue / maxValue;
        healthBar.value = Mathf.Lerp(healthBar.value, percentage, Time.deltaTime * healthBarDropSpeed);
    }

    public void EnableHealthBar()
    {
        healthBar.enabled = true;
    }

    public void DisableHealthBar()
    {
        healthBar.enabled = false;
    }
}
