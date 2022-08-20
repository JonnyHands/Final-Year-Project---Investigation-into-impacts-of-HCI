using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShot : MonoBehaviour
{
    public float speed;
    private Vector3 direction;
    public Rigidbody2D bulletRigidbody;
    public GameObject impactEffect;
    public GameObject hitEffect;
    // Start is called before the first frame update
    void Start()
    {
        direction = transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        //makes shot move
        transform.position += direction * speed * Time.deltaTime;
        //checks if boss isnt in the game
        if (!Boss.instance.gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
        }
    }
    //method to check shot collides
    private void OnTriggerEnter2D(Collider2D other)
    {
        //checks if it collides with player
        if (other.tag == "Player")
        {
            //checks if player is invincible
            if (!PlayerHealth.instance.GetIsInvincible())
            {
                //damages player and creates effect
                PlayerHealth.instance.DamagePlayer(2);
                Instantiate(hitEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            //if player is invincible destroys bullet
            else if (PlayerHealth.instance.GetIsInvincible())
            {
                Destroy(gameObject);
            }
        }

        else
        { //if it hits anything else makes an impact effect
            Instantiate(impactEffect, transform.position, transform.rotation);
            AudioManager.instance.PlaySFX(3);
            Destroy(gameObject);
        }
    }

    //if boss is invisible shots destroyed
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
