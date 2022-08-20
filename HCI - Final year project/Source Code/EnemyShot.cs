using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    public float speed;
    private Vector3 direction;
    public Rigidbody2D bulletRigidbody;
    public GameObject impactEffect;
    public GameObject hitEffect;
    // Start is called before the first frame update
    void Start()
    {
        direction = Player.instance.transform.position - transform.position;
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position +=direction * speed * Time.deltaTime;
    }

    //method for collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        //checks if collision was with player
        if(other.tag == "Player")
        {
            //checks if player was not invincible, if so damages the player and creates effect
            if (!PlayerHealth.instance.GetIsInvincible())
            {
                PlayerHealth.instance.DamagePlayer(1);
                Instantiate(hitEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            //checks if the player was invincible if so shot is destroyed
            else if (PlayerHealth.instance.GetIsInvincible())
            {
                Destroy(gameObject);
            }
        }
        //otherwise bullet impact effect is produced
        else
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            AudioManager.instance.PlaySFX(3);
            Destroy(gameObject);
        }
    }

    //method for invisibility
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
