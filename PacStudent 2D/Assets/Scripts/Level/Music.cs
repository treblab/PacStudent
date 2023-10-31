using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    [SerializeField] private AudioSource introBGM;
    [SerializeField] private AudioSource ghostBGM;

    void Update()
    {
        Debug.Log("Intro BGM is playing: " + introBGM.isPlaying);
        if (!introBGM.isPlaying && !ghostBGM.isPlaying)
        {
            ghostBGM.Play();
        }
        
    }
}
