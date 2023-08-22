using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] soundEffect;

    private void Awake()
    {
        instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
