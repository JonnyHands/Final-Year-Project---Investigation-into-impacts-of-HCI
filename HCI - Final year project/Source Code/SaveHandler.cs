using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveHandler : MonoBehaviour
{
    public static SaveHandler save;

    public int currentHealth;
    public int maxHealth;
    public int currentCoins;
    public float numberOfShots;
    public float fireRate;
    public int damageToGive;
    // Start is called before the first frame update

        //initaites place to store local save
    private void Awake()
    {
        save = this;
        maxHealth = PlayerPrefs.GetInt("health");
        currentHealth = PlayerPrefs.GetInt("health");
    }

    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
