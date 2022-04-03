using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager instance;
    public AudioClip[] hurtClips;
    public AudioClip levelUpClip;
    public AudioClip loseClip;

    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHurtSound()
    {
        if(audioSource.isPlaying)
        {
            return;
        }

        //Get a random clip
        int index = Random.Range(0, hurtClips.Length);
        AudioClip clip = hurtClips[index];
        audioSource.clip = clip;
        audioSource.pitch = Random.Range(0.95f, 1.1f); //Randomize the sound
        audioSource.Play();
    }

    public void PlayLevelUpSound()
    {
        audioSource.clip = levelUpClip;
        audioSource.pitch = 1f;
        audioSource.Play();
    }

    public void PlayLoseSound()
    {
        audioSource.clip = loseClip;
        audioSource.pitch = 1f;
        audioSource.Play();
    }
}
