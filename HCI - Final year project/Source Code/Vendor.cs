using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : MonoBehaviour
{
    public GameObject prompt;
    public bool isRestore, isUpgrade,isFireRate,isDamage,isAbility;
    public bool isSoul, isLife;
    public int cost;
    public int lifecost;
    private bool inVendor;
    public bool tutorial = false;
    // Start is called before the first frame update
    void Start()
    {
        inVendor = false;
    }

    // Update is called once per frame
    void Update()
    {
        //checks if shop
        if (inVendor)
        {
            //checks for e key pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                //checks shop type
                if (isSoul)
                {
                    //checks souls compared to cost
                    if (LevelManager.instance.amountOfSouls >= cost)
                    {
                        LevelManager.instance.SpendSouls(cost);
                        //checks if it restores health if so does so
                        if (isRestore)
                        {
                            PlayerHealth.instance.HealPlayer(PlayerHealth.instance.maxHealth);
                        }
                        //checks if upgrades health if so does so
                        if (isUpgrade)
                        {
                            PlayerHealth.instance.maxHealth += 1;
                            PlayerHealth.instance.SaveHealth();
    
                            UIController.instance.healthSlider.maxValue = PlayerHealth.instance.maxHealth;
                            UIController.instance.healthSlider.value = PlayerHealth.instance.currentHealth;
                            UIController.instance.healthText.text = PlayerHealth.instance.currentHealth.ToString() + " / " + PlayerHealth.instance.maxHealth.ToString();

                        }
                        gameObject.SetActive(false);
                        inVendor = false;
                        AudioManager.instance.PlaySFX(11);
                    }
                }
                //checks shop type is life sacrifice
                if (isLife)
                {
                    //checks palyers health to the cost
                    if (PlayerHealth.instance.currentHealth >= lifecost)
                    {
                        PlayerHealth.instance.DamagePlayer(lifecost);
                        //upgrades fireate
                        if (isFireRate)
                        {
                            Player.instance.fireRate -= 0.05f;
                        }
                        //upgrades damage
                        if (isDamage)
                        {
                            Player.instance.damageToGive +=5;

                        }
                        //adds ability
                        if (isAbility)
                        {
                            Player.instance.numberOfShots += 1;
                        }
                        gameObject.SetActive(false);
                        inVendor = false;
                        AudioManager.instance.PlaySFX(11);
                    }
                }
                else
                {
                    AudioManager.instance.PlaySFX(12);
                }
                
            }
        }
    }

    //checks collision with player if so activates key prompt
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inVendor = true;
            prompt.SetActive(true);
            if (tutorial)
            {
                AudioManager.instance.PlaySFX(12);
            }
        }
    }

    //chekcs player has left, removes prompt
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inVendor = false;
            prompt.SetActive(false);
        }
    }


}
