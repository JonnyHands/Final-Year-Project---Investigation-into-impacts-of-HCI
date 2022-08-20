using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public string levelToLoad;
    public string loadTutorial;
    public GameObject canvas;
    public GameObject settingsMenu;
    public GameObject delete;
    public GameObject dialog;
    public static MainMenuHandler instance;
    public bool isSave;
    public Text dialogText;
    public Text saveText;
    //bool settingsOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        isSave = PlayerHealth.instance.isSave;
    }

    // Update is called once per frame
    void Update()
    {
        //make ssure current save slot is forever accurate
        saveText.text = ("Current Save Max Health: " + PlayerPrefs.GetInt("health")).ToString();
    }

    //method to start game checks if there is a save and loads it if not creates new save
    public void StartGame()
    {
        //checks for gamesave if so loads save
        if (isSave)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(levelToLoad);
            PlayerHealth.instance.maxHealth = PlayerPrefs.GetInt("health");
        }
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(levelToLoad);
            PlayerPrefs.SetInt("health", 5);
            isSave = true;
        }
        
        
    }

    //method to delete players current save
    public void DeleteSave()
    {
        delete.SetActive(false);
        dialog.SetActive(true); 
        dialogText.text = ("Current save has a max health of:   " + PlayerPrefs.GetInt("health")).ToString();
    }

    //confirmation box popup for delete save dialog
    public void ConfirmYes()
    {
        PlayerPrefs.SetInt("health", 5);
        isSave = false;
        PlayerHealth.instance.isSave = false;
        dialog.SetActive(false);

    }
    //confirmation box popup for delete save dialog
    public void ConfirmNo()
    {
        delete.SetActive(true);
        dialog.SetActive(false);
    }
    //method to launch games tutorial
    public void StartTutorial()
    {
        if (isSave)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(loadTutorial);
            PlayerHealth.instance.maxHealth = PlayerPrefs.GetInt("health");
        }
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(loadTutorial);
            PlayerPrefs.SetInt("health", 5);
            isSave = true;
        }
    }
    //method to open settings from main menu
    public void Settings()
    {        
       ToggleSettings.instance.OpenSettings(false);
    }

    //method to exit the game
    public void ExitGame()
    {
        Application.Quit();
    }


}
