using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Slider healthSlider;
    public Text healthText;
    public GameObject dashUI;
    public Slider dashSlider;
    public Text dashCoolText;
    public Text colourblindText;
    public static UIController instance;
    public GameObject deathScreen;
    public Image fadeScreen;
    public float fadeSpeed = 1f;
    private bool fadeIn;
    private bool fadeOut;
    public string newGameScene, mainMenuScene;
    public GameObject pauseMenu;
    public GameObject helpMenu;
    public GameObject settingsMenu;
    public Text coinText;
    public GameObject minimap;
    public GameObject bigmap;
    public GameObject minimapcam;
    public GameObject bigmapcam;
    public GameObject bossHealth;
    public string loadTutorial;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        fadeOut = true;
        fadeIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        //checks if fade is needed, if so adds filter to fsde image over scene as it loads
        if (fadeOut)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 0f)
            {
                fadeOut = false;
            }
        }

        //same as above but in reverse
        if (fadeIn)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                fadeIn = false;
            }
        }
    }

    //sets booleans for screen fades
    public void StartLevelFade()
    {
        fadeIn = true;
        fadeOut = false;
    }

    //handles tutorial scene loading
    public void StartTutorial()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(loadTutorial);
    }

    //handles newgame scene loading
    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(newGameScene);
    }


    public void MainMenu()
    {
        PlayerHealth.instance.SaveHealth();
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
        MainMenuHandler.instance.isSave = true;
    }
 
    public void Resume()
    {
        LevelManager.instance.PauseResume();
    }
    
    public void ResumeSettings()
    {
        LevelManager.instance.Resume();
    }
    public void Back()
    {
        LevelManager.instance.Back();
    }
    public void Settings()
    {
        LevelManager.instance.Settings();
    }

    public void Stuck()
    {
        LevelManager.instance.Stuck();
    }

    public void ColourBlind()
    {
        LevelManager.instance.ColourBlind();
    }
}
