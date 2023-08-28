using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleIndicator : MonoBehaviour
{
    [SerializeField] private Image meleeImage;
    [SerializeField] private Image rangeImage;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.Instance;
    }

    public void ToggleAttackType(bool isRange)
    {
        audioManager.PlayUISound("Toggle");
        
        if (isRange)
        {
            meleeImage.gameObject.SetActive(false);
            rangeImage.gameObject.SetActive(true);
        }
        else
        {
            meleeImage.gameObject.SetActive(true);
            rangeImage.gameObject.SetActive(false);
        }
    }
}
