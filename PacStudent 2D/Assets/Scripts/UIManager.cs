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

    // Update is called once per frame
    void Update()
    {
        
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
}
