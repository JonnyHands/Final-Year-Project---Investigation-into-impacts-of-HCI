using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //method to check if player has just collided with gameobject, if so and not invincible, damages them
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !PlayerHealth.instance.GetIsInvincible())
        { if (!PlayerHealth.instance.GetIsInvincible())
                PlayerHealth.instance.DamagePlayer(1);
        }
    }
    //method to check if player hasn't left collison with gameobject, if so and not invincible, damages them
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && !PlayerHealth.instance.GetIsInvincible())
        {
            PlayerHealth.instance.DamagePlayer(1);
        }
    }

    //method to check if player has entered a collision with gameobject, if so and not invincible, damages them
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !PlayerHealth.instance.GetIsInvincible())
        {
            PlayerHealth.instance.DamagePlayer(1);
        }
    }

    //method to check if player has stayed in collision with gameobject, if so and not invincible, damages them
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !PlayerHealth.instance.GetIsInvincible())
        {
            PlayerHealth.instance.DamagePlayer(1);
        }
    }
}
