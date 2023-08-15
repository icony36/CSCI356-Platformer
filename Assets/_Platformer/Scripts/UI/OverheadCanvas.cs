using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LookAtCamera))]
public class OverheadCanvas : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Combat combat;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image healthBarOutline;
    [SerializeField] private Image healthFill;

    [Header("Settings")]
    [SerializeField] private float healthBarDropSpeed = 50f;

    private void Start()
    {   
        if (combat.gameObject.tag == "Enemy")
        {
            healthBarOutline.color = combat.EnemyColor; 
            healthFill.color = combat.EnemyColor;
        }
        else
        {
            healthBarOutline.color = combat.PlayerColor;
            healthFill.color = combat.PlayerColor;
        }
    }

    private void Update()
    {
        float percentage = (float)combat.CurrentHealth / combat.MaxHealth;
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
