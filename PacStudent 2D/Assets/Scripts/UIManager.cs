using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // 60% Band - In game UI Appearance:
    public RectTransform startScreen;
    public RectTransform gameHUD;
    public RectTransform ghost1Canvas;
    public RectTransform ghost2Canvas;
    public RectTransform ghost3Canvas;
    public RectTransform ghost4Canvas;

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
            Debug.Log("gameHUD sizeDelta called.");
        }

        if (ghost1Canvas != null)
        {
            ghost1Canvas.sizeDelta = new Vector2(Screen.width, Screen.height);
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
}
