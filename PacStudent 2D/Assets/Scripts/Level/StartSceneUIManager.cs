using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneUIManager : MonoBehaviour
{
    public RectTransform startScreen;
    public Text highScoreText; 
    public Text highScoreTimeText;

    void Start()
    {
        if (startScreen != null)
        {
            startScreen.sizeDelta = new Vector2(Screen.width, Screen.height);
        }

        LoadHighScoreAndTime();
    }

    private void LoadHighScoreAndTime()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        string highScoreTime = PlayerPrefs.GetString("LastTime", " ");

        highScoreText.text = "High Score: " + highScore.ToString();
        highScoreTimeText.text = highScoreTime;
    }

    public void LoadLevelOne()
    {
        SceneManager.LoadSceneAsync(1);
    }
}

