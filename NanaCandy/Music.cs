using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    Animator anim;
    AudioSource audioSource;

    void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void MusicToggle()
    {
        if (anim.GetBool("MusicOff"))
        {
            anim.SetBool("MusicOff", false);
            audioSource.Play();
        } else
        {
            anim.SetBool("MusicOff", true);
            audioSource.Stop();
        }
    }
}
