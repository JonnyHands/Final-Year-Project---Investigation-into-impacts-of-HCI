using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public float loadWait = 3f;
    public string nextLevel;
    public bool isPaused;
    public bool isHelp;
    public int amountOfSouls;
    public bool isMap;
    public bool cbON = false;
    public string on = ("Colour Blind Mode : " + "ON");
    public string off = ("Colour Blind Mode : " + "OFF");
    public bool isBoss;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        amountOfSouls = SaveHandler.save.currentCoins;
        UIController.instance.colourblindText.text = off.ToString();
        Time.timeScale = 1f;
        UIController.instance.coinText.text = amountOfSouls.ToString();
    }

    // this update method handles all the level management and UI inititations and actions
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseResume();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && !isBoss)
        {
            ToggleMap();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(PlayerPrefs.GetInt("health".ToString()));
        }
    }


    //method to play correct music depending on scene loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.CompareTo("Level1") == 0)
        {
            AudioManager.instance.PlayGameMusic();
        }
        if (scene.name.CompareTo("Level2") == 0)
        {
            AudioManager.instance.PlayGameMusic();
        }
        if (scene.name.CompareTo("Level3") == 0)
        {
            AudioManager.instance.PlayGameMusic();
        }
        if (scene.name.CompareTo("Boss") == 0)
        {
            AudioManager.instance.PlayBossMusic();
        }
    }

    //ienumerator to end level store stats and move on
    public IEnumerator LevelEnd()
    {
        Player.instance.canMove = false;
        UIController.instance.StartLevelFade();
        yield return new WaitForSeconds(loadWait);
        SceneManager.LoadScene(nextLevel);
        SaveHandler.save.currentCoins = amountOfSouls;
        SaveHandler.save.currentHealth = PlayerHealth.instance.currentHealth;
        SaveHandler.save.maxHealth = PlayerHealth.instance.maxHealth;
        SaveHandler.save.numberOfShots = Player.instance.numberOfShots;
        SaveHandler.save.damageToGive = Player.instance.damageToGive;
        SaveHandler.save.fireRate = Player.instance.fireRate;
    }

    //method for the pausing emthod, handles timescale so it pauses aswell as opens pausing UI
    public void PauseResume()
    {
        if (!isPaused)
        {
            UIController.instance.pauseMenu.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
        else
        {
            UIController.instance.pauseMenu.SetActive(false);
            isPaused = false;
            UIController.instance.settingsMenu.SetActive(false);
            isHelp = false;
            UIController.instance.helpMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    //method to handle and open settings for the game
    public void Settings()
    {
        isHelp = true;
        Time.timeScale = 0f;
        UIController.instance.pauseMenu.SetActive(false);
        UIController.instance.helpMenu.SetActive(true);
    }
    //method to go back to previous UI
    public void Back()
    {
        UIController.instance.helpMenu.SetActive(false);
        UIController.instance.pauseMenu.SetActive(true);
    }
    //method to resume the game
    public void Resume()
    {
        UIController.instance.helpMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    //method to update souls UI text
   public void ReceiveSouls(int x)
    {
        amountOfSouls += x;
        UIController.instance.coinText.text = amountOfSouls.ToString();
    }

    //method for if the player bugs and gets stuck, was used for development but was made a feature
    public void Stuck()
    {
        Player.instance.rigidBody.position = new Vector2(0,0);
        UIController.instance.settingsMenu.SetActive(false);
        UIController.instance.pauseMenu.SetActive(false);
        UIController.instance.helpMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        var doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (var d in doors)
        {
            d.SetActive(false);
        }
    }

    //method that implements the colourblind package for accesibility
    public void ColourBlind()
    {
        if (!cbON)
        {
            UIController.instance.colourblindText.text = on.ToString();
            cbON = true;
            Colourblind.instance.Type = 3;
        }
        else
        {
            UIController.instance.colourblindText.text = off.ToString();
            cbON = false;
            Colourblind.instance.Type = 0;
        }
    }
    //method to update souls count when spent
    public void SpendSouls(int x)
    {
        amountOfSouls -= x;
        UIController.instance.coinText.text = amountOfSouls.ToString();        
    }

    //method to toggle big and small  minimap UI
    public void ToggleMap()
    {
        if (!isMap)
        {
            UIController.instance.bigmap.SetActive(true);
            UIController.instance.minimap.SetActive(false);
            UIController.instance.bigmapcam.SetActive(true);
            UIController.instance.minimapcam.SetActive(false);
            isMap = true;
        }
        else
        {
            UIController.instance.bigmap.SetActive(false);
            UIController.instance.minimap.SetActive(true);
            UIController.instance.bigmapcam.SetActive(false);
            UIController.instance.minimapcam.SetActive(true);
            isMap = false;
        }
    }
}
