using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    [SerializeField] private AudioSource introBGM;
    [SerializeField] private AudioSource ghostBGM;

    // Start is called before the first frame update
    void Start()
    {
        introBGM.enabled = true;
        ghostBGM.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (introBGM.isPlaying == false)
        {
            ghostBGM.enabled = true;
            ghostBGM.loop = true;
        }
        
    }
}
