using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneUIManager : MonoBehaviour
{
    public RectTransform startScreen;
    public Text highScoreText; 
    public Text highScoreTimeText;
    public Image BottomCoffeeImage;
    public Image BottomHotCoffeeImage;
    private Tweener tweener;

    void Start()
    {
        if (startScreen != null)
        {
            startScreen.sizeDelta = new Vector2(Screen.width, Screen.height);
        }
        AnimatedBorder();
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

    private void AnimatedBorder()
    {
        tweener = BottomCoffeeImage.GetComponent<Tweener>();
        tweener = BottomHotCoffeeImage.GetComponent<Tweener>();

        StartCoroutine(TweenLoopCoffee());
        StartCoroutine(TweenLoopHotCoffee());
    }

    private IEnumerator TweenLoopCoffee()
    {
        while (true) // Loop indefinitely
        {
            Vector3 startPos = BottomCoffeeImage.transform.position;
            Vector3 endPos = new Vector3(1900,100);

            // Tween to the end position
            tweener.AddTween(BottomCoffeeImage.transform, startPos, endPos, 2.0f);
            yield return new WaitForSeconds(3.0f); // Wait for the tween to complete

            // Tween back to the start position
            tweener.AddTween(BottomCoffeeImage.transform, endPos, startPos, 2.0f);
            yield return new WaitForSeconds(3.0f); // Wait for the tween to complete
        }
    }

    private IEnumerator TweenLoopHotCoffee()
    {
        while (true) // Loop indefinitely
        {
            Vector3 startPos = BottomHotCoffeeImage.transform.position;
            Vector3 endPos = new Vector3(-105, 100);

            // Tween to the end position
            tweener.AddTween(BottomHotCoffeeImage.transform, startPos, endPos, 2.0f);
            yield return new WaitForSeconds(3.0f); // Wait for the tween to complete

            // Tween back to the start position
            tweener.AddTween(BottomHotCoffeeImage.transform, endPos, startPos, 2.0f);
            yield return new WaitForSeconds(3.0f); // Wait for the tween to complete
        }
    }

}

