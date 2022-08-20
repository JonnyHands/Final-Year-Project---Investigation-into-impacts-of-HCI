using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    public int healAmount = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  
    //if player touches pickup, pickup is removed and heals player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            AudioManager.instance.PlaySFX(5);
            PlayerHealth.instance.HealPlayer(healAmount);
            Destroy(gameObject);
        }
    }
}
