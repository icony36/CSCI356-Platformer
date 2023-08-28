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

    public void ToggleImage(bool isMelee)
    {
        if (isMelee)
        {
            meleeImage.gameObject.SetActive(true);
            rangeImage.gameObject.SetActive(false);
        }
        else
        {
            meleeImage.gameObject.SetActive(false);
            rangeImage.gameObject.SetActive(true);
        }
    }
}
