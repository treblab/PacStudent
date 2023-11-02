using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GhostController : MonoBehaviour
{
    [SerializeField] private List<Animator> ghostAnimators; 
    [SerializeField] private AudioSource backgroundMusic; 
    [SerializeField] private AudioClip scaredMusic; 
    [SerializeField] private AudioClip normalMusic; 
    [SerializeField] private Text ghostTimer; 
    [SerializeField] private float scaredDuration = 10f;

    private UIManager uiManager;

    void Start()
    {
        uiManager = GameObject.Find("Managers").GetComponent<UIManager>();
    }

    public void PowerPelletEaten()
    {
        // Change ghost state to "Scared"
        foreach (Animator ghostAnimator in ghostAnimators)
        {
            ghostAnimator.SetBool("isScared", true);
        }

        // Change the background music to match the "Scared" state
        backgroundMusic.clip = scaredMusic;
        backgroundMusic.Play();

        // Start the scared timer
        StartCoroutine(ScaredTimer());
    }

    private IEnumerator ScaredTimer()
    {
        // Make the Ghost Timer UI element visible and set it to the scaredDuration
        // ghostTimer.enabled = true;
        float timer = scaredDuration;
        // ghostTimer.text = "Ghosts Scared For: 10s";

        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            // ghostTimer.text = "Ghosts Scared For: " + timer + "s";
            // Debug.Log("Ghost timer: " + timer);

            // With 3 seconds left, change the Ghosts to the "Recovering" state
            if (timer == 3)
            {
                foreach (Animator ghostAnimator in ghostAnimators)
                {
                    ghostAnimator.SetBool("isScared", false);
                    ghostAnimator.SetBool("isRecovering", true);
                }
            }
        }

        // After the timer ends, set the Ghosts back to their "Walking" states
        foreach (Animator ghostAnimator in ghostAnimators)
        {
            ghostAnimator.SetBool("isRecovering", false);
            ghostAnimator.SetBool("isNormal", true);
        }

        // Hide the Ghost Timer UI element and revert the background music
        // ghostTimer.enabled = false;
        backgroundMusic.clip = normalMusic;
        backgroundMusic.Play();
    }
}
