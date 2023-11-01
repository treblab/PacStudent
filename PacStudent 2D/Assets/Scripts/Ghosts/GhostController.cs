using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostController : MonoBehaviour
{
    [SerializeField] private List<Animator> ghostAnimators; 
    [SerializeField] private AudioSource backgroundMusic; 
    [SerializeField] private AudioClip scaredMusic; 
    [SerializeField] private AudioClip normalMusic; 
    [SerializeField] private GameObject ghostTimerUI; 
    [SerializeField] private float scaredDuration = 10f; 

    public void OnPowerPelletEaten()
    {
        // Change ghost state to "Scared"
        foreach (Animator ghostAnimator in ghostAnimators)
        {
            ghostAnimator.SetTrigger("Scared");
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
        ghostTimerUI.SetActive(true);
        float timer = scaredDuration;

        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;

            // With 3 seconds left, change the Ghosts to the "Recovering" state
            if (timer == 3)
            {
                foreach (Animator ghostAnimator in ghostAnimators)
                {
                    ghostAnimator.SetTrigger("Recovering");
                }
            }
        }

        // After the timer ends, set the Ghosts back to their "Walking" states
        foreach (Animator ghostAnimator in ghostAnimators)
        {
            ghostAnimator.SetTrigger("Walking");
        }

        // Hide the Ghost Timer UI element and revert the background music
        ghostTimerUI.SetActive(false);
        backgroundMusic.clip = normalMusic;
        backgroundMusic.Play();
    }
}
