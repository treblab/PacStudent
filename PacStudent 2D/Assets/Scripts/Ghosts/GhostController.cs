using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GhostController : MonoBehaviour
{
    [SerializeField] private List<GameObject> ghostObjects;
    
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioClip[] ghostMusicClips;
    
    [SerializeField] private Text ghostTimer; 
    [SerializeField] private float scaredDuration = 10f;

    private Coroutine scaredTimerCoroutine;

    public void PowerPelletEaten()
    {
        // Here to refresh the countdown if another pellet is eaten before the countdown finishes.
        if (scaredTimerCoroutine != null)
        {
            StopCoroutine(scaredTimerCoroutine);
        }

        // Change ghost state to "Scared"
        foreach (GameObject ghost in ghostObjects)
        {
            ghost.GetComponent<Animator>().SetBool("isRecovering", false);
            ghost.GetComponent<Animator>().SetBool("isNormal", false);
            ghost.GetComponent<Animator>().SetBool("isScared", true);
            ghost.tag = "scaredGhost";
        }

        // Change the background music to match the "Scared" state
        backgroundMusic.clip = ghostMusicClips[1];
        backgroundMusic.Play();

        // Start the scared timer
        scaredTimerCoroutine = StartCoroutine(ScaredTimer());
    }

    private IEnumerator ScaredTimer()
    {
        float timer = scaredDuration;

        // When method is called, set ghosts to be Scared.

        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;

            // With 3 seconds left, change the Ghosts to the "Recovering" state
            if (timer == 3)
            {
                foreach (GameObject ghost in ghostObjects)
                {
                    ghost.GetComponent<Animator>().SetBool("isScared", false);
                    ghost.GetComponent<Animator>().SetBool("isRecovering", true);

                    ghost.tag = "recoveringGhost";
                }
            }
        }

        // After the timer ends, set the Ghosts back to their "Walking" states
        foreach (GameObject ghost in ghostObjects)
        {
            ghost.GetComponent<Animator>().SetBool("isRecovering", false);
            ghost.GetComponent<Animator>().SetBool("isNormal", true);
            ghost.tag = "normalGhost";
        }

        // Hide the Ghost Timer UI element and revert the background music
        // ghostTimer.enabled = false;
        backgroundMusic.clip = ghostMusicClips[0];
        backgroundMusic.Play();
    }

    public void scaredGhostEaten(GameObject ghostEaten)
    {
        var ghostAnimator = ghostEaten.GetComponent<Animator>();
        if (ghostAnimator != null)
        {
            ghostAnimator.SetBool("isScared", false);
            ghostAnimator.SetBool("isRecovering", false);
            ghostAnimator.SetBool("isNormal", false);
            ghostAnimator.SetBool("isDead", true);
        }
        ghostEaten.tag = "deadGhost";

        backgroundMusic.clip = ghostMusicClips[2];
        backgroundMusic.Play();
        StartCoroutine(RevertGhostToNormal(ghostEaten));
    }

    // Coroutine to turn the ghost back to normal after ~5s
    private IEnumerator RevertGhostToNormal(GameObject ghost)
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Check if the ghost still exists (hasn't been destroyed or deactivated)
        if (ghost != null)
        {
            var ghostAnimator = ghost.GetComponent<Animator>();
            if (ghostAnimator != null)
            {
                // Revert the ghost's Animator states and tag to normal
                ghostAnimator.SetBool("isDead", false);
                ghostAnimator.SetBool("isNormal", true);
            }
            ghost.tag = "normalGhost";
            Debug.Log("Ghost back to normal. ");
        }
    }
}
