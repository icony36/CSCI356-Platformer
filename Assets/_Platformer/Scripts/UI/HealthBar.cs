using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Slider healthBar;

    [Header("Settings")]
    [SerializeField] private float currentValue;
    [SerializeField] private float maxValue;
    [SerializeField] private float healthBarDropSpeed = 50f;
    

    void Start()
    {
      
    }

    void Update()
    {
        float percentage = (float)playerData.currentHealth / playerData.maxHealth;
        healthBar.value = Mathf.Lerp(healthBar.value, percentage, Time.deltaTime * healthBarDropSpeed);
    }
}
