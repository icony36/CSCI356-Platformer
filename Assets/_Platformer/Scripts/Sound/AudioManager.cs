using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource[] soundEffect;

    private void Awake()
    {
        Instance = this; 
    }

    public void PlaySFX(int sfxNumber)
    {
        soundEffect[sfxNumber].Stop();
        soundEffect[sfxNumber].Play();
    }

    public void StopSFX(int sfxNumber)
    {
        soundEffect[sfxNumber].Stop();
    }
}
