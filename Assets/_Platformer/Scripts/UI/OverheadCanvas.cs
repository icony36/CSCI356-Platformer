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
    [SerializeField] private Color healthbarColor = new Color(0, 255, 0);
    [SerializeField] private float currentValue;
    [SerializeField] private float maxValue;
    [SerializeField] private float healthBarDropSpeed = 50f;

    private void Start()
    {
        parent = gameObject.transform.parent.gameObject;
        healthBarOutline.color = healthbarColor;
        healthFill.color = healthbarColor;
    }

    private void Update()
    {
        if (parent.tag == "Enemy")
        {
            currentValue = parent.GetComponent<EnemyCombat>().CurrentHealth;
            maxValue = parent.GetComponent<EnemyCombat>().MaxHealth;
        }
        else if (parent.tag == "Player")
        {
            currentValue = parent.GetComponent<Player>().playerData.currentHealth;
            maxValue = parent.GetComponent<Player>().playerData.maxHealth;
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
