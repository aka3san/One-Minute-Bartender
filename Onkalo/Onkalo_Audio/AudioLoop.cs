using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoop : MonoBehaviour
{
    [SerializeField] AudioSource intro;
    [SerializeField] AudioSource loop;

    void Awake(){
        intro.volume = AudioSettingManager.Audio_BGM();
        loop.volume = AudioSettingManager.Audio_BGM();
        loop.PlayDelayed(10.663f);
    }

    void Start(){
        // loop.PlayDelayed(10.663f);
    }


    // void Start()
    // {
    //     intro.volume = AudioSettingManager.Audio_BGM();
    //     loop.volume = AudioSettingManager.Audio_BGM();
    //     intro.PlayScheduled(AudioSettings.dspTime);
    //     loop.PlayScheduled (AudioSettings.dspTime + ((float)intro.clip.samples / (float)intro.clip.frequency));
    // }

    // void Update(){
    //     if()
    // }
}

