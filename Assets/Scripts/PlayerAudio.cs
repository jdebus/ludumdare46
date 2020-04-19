using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip walkSound;
    public AudioClip eatSound;
    public AudioClip shootSound;

    public void OnWalkSound()
    {
        PlayClip(walkSound);    
    }

    public void OnEatSound()
    {

        PlayClip(eatSound);
    }

    public void OnShootSound()
    {
        PlayClip(shootSound);
    }


    void PlayClip(AudioClip clip)
    {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }
}
