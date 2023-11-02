using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 60% Band - In game UI Appearance:
    public RectTransform startScreen;

    // 80% Band - Collisions, UI updates and Saving High Scores:
    public Text ghostTimer;
    private Button exitButton;
    public Text scoreText;
    public Image[] lives;
    private int amountOfLives = 3;
    private int currentScore = 0;
    private Coroutine ghostTimerCoroutine;
    public Text roundStartTimer;
    public GameObject pacStudent;
    private PacStudentController pacStudentController;
    public Timer gameTimer;
    public Text highScoreText;

    void Awake()
    {
         StartCoroutine(RoundStartCountdown()); // FOR TESTING DELETE BEFORE SUBMISSION

        if (pacStudent != null)
        {
            pacStudentController = pacStudent.GetComponent<PacStudentController>();
        }

        if (startScreen != null)
        {
            startScreen.sizeDelta = new Vector2(Screen.width, Screen.height);
        }

        if (ghostTimer != null)
        {
            ghostTimer.enabled = false;
        }

    }

    public void LoadLevelOne()
    {
        DontDestroyOnLoad(this);
        SceneManager.LoadSceneAsync(1);
        StartCoroutine(RoundStartCountdown());
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

    private IEnumerator RoundStartCountdown()
    {
        if (!roundStartTimer)
        {
            Debug.Log("Round start timer is null! ");
        }

        roundStartTimer.enabled = true; // Make sure the countdown text is visible

        // Countdown from 3 to 1 then show "GO!"
        roundStartTimer.text = "3";
        yield return new WaitForSeconds(1f);

        roundStartTimer.text = "2";
        yield return new WaitForSeconds(1f);

        roundStartTimer.text = "1";
        yield return new WaitForSeconds(1f);

        roundStartTimer.text = "GO!";
        yield return new WaitForSeconds(1f);

        // Hide the countdown text and start the game, allowing pacstudent to move.
        roundStartTimer.enabled = false;
        pacStudentController.togglePacStudentMovement(true);
        gameTimer.StartTimer();

        // Start the background music
        // backgroundMusic.Play();

        // Start the game timer if you have one
        // StartGameTimer();
    }

    private void updateHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        string highScoreTime = PlayerPrefs.GetString("HighScoreTime", "00:00:00");
        highScoreText.text = "High Score: " + highScore + " Time: " + highScoreTime;
    }
}
