using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Combat combat;
    [SerializeField] private Slider healthBar;

    [Header("Settings")]
    [SerializeField] private float healthBarDropSpeed = 50f;

    void Start()
    {
    }

    void Update()
    {
        float percentage = (float)combat.CurrentHealth / combat.MaxHealth;
        healthBar.value = Mathf.Lerp(healthBar.value, percentage, Time.deltaTime * healthBarDropSpeed);
    }
}
