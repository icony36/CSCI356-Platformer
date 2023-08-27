using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAudio : MonoBehaviour, IPointerEnterHandler
{
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.Instance;
    }

    public void OnPointerEnter(PointerEventData ped)
    {
        audioManager.PlayUISound("Hover");
    }

    public void PlayClickSound()
    {
        audioManager.PlayUISound("Click");
    }

    public void PlayCloseSound()
    {
        audioManager.PlayUISound("Close");
    }
}
