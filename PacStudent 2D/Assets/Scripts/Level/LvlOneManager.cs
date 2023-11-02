using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LvlOneManager : MonoBehaviour
{
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
    public Text gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RoundStartCountdown());

        if (pacStudent != null)
        {
            pacStudentController = pacStudent.GetComponent<PacStudentController>();
        }

        if (ghostTimer != null)
        {
            ghostTimer.enabled = false;
        }

        if (gameOverText != null)
        {
            gameOverText.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (amountOfLives == 0 || pacStudentController.eatenAllPellets())
        {
            GameOver();
        }
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
        if (scoreText != null)
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
        // highScoreText.text = "High Score: " + highScore + " Time: " + highScoreTime;
    }

    private void SaveHighScoreAndTime()
    {
        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);
        string savedTime = PlayerPrefs.GetString("HighScoreTime", "00:00:00");

        bool newRecord = false;
        if (currentScore > savedHighScore)
        {
            newRecord = true;
        }
        else if (currentScore == savedHighScore)
        {
            if (string.CompareOrdinal(gameTimer.GetCurrentTime(), savedTime) < 0)
            {
                newRecord = true;
            }
        }
    }

    public void GameOver()
    {
        // Show Game Over Text
        gameOverText.enabled = true;

        // Stop all player and ghost movement
        pacStudentController.togglePacStudentMovement(false);
        // Stop all ghosts not needed as I did not get to the 90% band.

        // Pause the Game Timer
        gameTimer.StopTimer();

        // Save High Score and Time if conditions are met
        SaveHighScoreAndTime();

        // Wait for 3 seconds and load the Start Scene
        StartCoroutine(WaitAndLoadStartScene());
    }

    private IEnumerator WaitAndLoadStartScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("StartScene");
        updateHighScore();
    }

}
