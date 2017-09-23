using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    float transitionTime;
     
    [SerializeField]
    AudioSource[] levelAudio;

    [SerializeField]
    AudioMixerSnapshot[] levelSnap;

    void Start()
    {
        if(instance)
        {
            //GameManager exists,delete copy
            DestroyImmediate(gameObject);
        }
        else
        {
            //assign GameManager to variable "_instance"
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public void fadeToAudio(int snap)
    {

        for (int count = 0; count < this.levelAudio.Length; count++)
        {
            this.levelAudio[count].Stop();
        }

        this.levelAudio[snap].Play();
        this.levelSnap[snap].TransitionTo(transitionTime);
    }


    public void fadeToAudio(int snap, float fadeTime)
    {
        this.levelAudio[snap].Play();
        this.levelSnap[snap].TransitionTo(fadeTime);
    }
}
