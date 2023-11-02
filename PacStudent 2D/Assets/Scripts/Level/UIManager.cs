using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 60% Band - In game UI Appearance:
    public RectTransform startScreen;
    public RectTransform gameHUD;
    public Text ghostTimer;
    private Button exitButton;
    public Text scoreText;

    public Image[] lives;
    private int amountOfLives = 3;

    private int currentScore = 0;
    private Coroutine ghostTimerCoroutine;

    // Start is called before the first frame update
    void Awake()
    {
        if (startScreen != null)
        {
            startScreen.sizeDelta = new Vector2(Screen.width, Screen.height);
            // Debug.Log("startScreen sizeDelta called.");
        }
        
        if (gameHUD != null)
        {
            gameHUD.sizeDelta = new Vector2(Screen.width, Screen.height);
            // Debug.Log("gameHUD sizeDelta called.");
        }

        if (ghostTimer != null)
        {
            ghostTimer.enabled = false;
            Debug.Log("Hiding ghostTimer called.");
        }

    }

    public void LoadLevelOne()
    {
        DontDestroyOnLoad(this);
        SceneManager.LoadSceneAsync(1);
    }

    public void exitToStart()
    {
        Debug.Log("Exiting to start scene..");
        SceneManager.LoadSceneAsync(0);
    }

    public void addScore(int points)
    {
        currentScore += points;
        updateScore();
    }

    public void updateScore()
    {
        if(scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString();
        }
    }

    public void startGhostTimer()
    {
        // Here to refresh the countdown if another pellet is eaten before the countdown finishes.
        if (ghostTimerCoroutine != null)
        {
            StopCoroutine(ghostTimerCoroutine);
        }
        ghostTimerCoroutine = StartCoroutine(GhostTimerCountdown(10f));
    }

    private IEnumerator GhostTimerCountdown(float duration)
    {

        ghostTimer.enabled = true;
        float remainingTime = duration;
        ghostTimer.text = "Ghosts scared for: 10s";

        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;
            ghostTimer.text = "Ghosts scared for: " + remainingTime + "s";

            // With 3 seconds left, do something to indicate the ghosts are recovering
            if (remainingTime <= 3)
            {
                // Change the UI to indicate recovering state
            }
        }

        ghostTimer.enabled = false;
        // Reset the ghosts to their normal state here or notify the GhostController to do so
    }

    public void removeLives()
    {
        --amountOfLives;
        if (amountOfLives >= 0)
        {
           lives[amountOfLives].enabled = false;
        }
    }
}
