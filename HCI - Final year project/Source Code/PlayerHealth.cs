using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    public int currentHealth;
    public int maxHealth;
    public float invincibilityLength = 1f;
    private float invincibilityCounter;
    private bool isInvincible;
    public AudioSource walk;
    public Scene scene;
    public bool isSave = false;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = PlayerPrefs.GetInt("health");
        currentHealth = SaveHandler.save.currentHealth;
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        //checks scene for audio
        if (scene.name == ("Boss"))
        {
            AudioManager.instance.PlayBossMusic();
        }
        else
        {
            AudioManager.instance.PlayGameMusic();
        }
    }

    // Update is called once per frame
    void Update()
    {
        scene = SceneManager.GetActiveScene();
        //checks if player is invincible
        if(invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
            if(invincibilityCounter <= 0)
            {
                isInvincible = false;
                Player.instance.bodySpriteRenderer.color = new Color(255, 255, 255, 1f);
            }
        }
        
    }

    //method to damage player
    public void DamagePlayer(int damage)
    {
        //checks if  isnt invinvible, if so adds audio and visual feedback
        if (invincibilityCounter <= 0)
        {
            AudioManager.instance.PlaySFX(8);
            currentHealth -= damage;
            invincibilityCounter = invincibilityLength;
            isInvincible = true;
            Player.instance.bodySpriteRenderer.color = new Color(255, 0, 0, .5f);
            //checks if health has hit 0 or lwoer if so player dies and mechanics for death are run
            if (currentHealth <= 0)
            {
                walk.Stop();
                Player.instance.gameObject.SetActive(false);
                UIController.instance.deathScreen.SetActive(true);
                UIController.instance.bossHealth.SetActive(false);
                AudioManager.instance.PlaySFX(7);
                AudioManager.instance.PlayDeathMusic();
                SaveHealth();
                //checks if died in boss scene if so runs method from boss script
                if (scene.name == ("Boss"))
                {
                    Boss.instance.PlayerDied();
                }
            }
            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }
    }

    //method to save health locally to computer
    public void SaveHealth()
    {
        PlayerPrefs.SetInt("health", maxHealth);
        isSave = true;
        MainMenuHandler.instance.isSave = true;

    }
    public bool GetIsInvincible()
    {
        return isInvincible;
    }

    public void MakeInvincible(float length)
    {
        invincibilityCounter = length;
    }

    //method to heal player from pickups
    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

}
