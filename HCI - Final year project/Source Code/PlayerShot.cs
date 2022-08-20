using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    public static PlayerShot instance;
    public float speed = 7.5f;
    public Rigidbody2D bulletRigidbody;
    public GameObject impactEffect;
    public GameObject hitEffect;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        bulletRigidbody.velocity = transform.right * speed;
    }

    //method to ahndle bulelt collisons
    private void OnTriggerEnter2D(Collider2D other)
    {
        //checks if bullet hit enemy
        if (other.tag == "Enemy" )
        {
            other.GetComponent<Enemy>().DamageEnemy(Player.instance.damageToGive);
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        //checks if bullet hit boss
        if (other.tag == "Boss" && !Boss.instance.isInvincible)
        {
            Boss.instance.TakeDamage(Player.instance.damageToGive);
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        //checks if bullet hit boss and is invincible
        if (other.tag == "Boss" && Boss.instance.bodySpriteRenderer.color == new Color(255, 0, 0, 0.5f))
        {
            Boss.instance.larmSpriteRenderer.color = new Color(255, 255, 0, 1f);
            Boss.instance.rarmSpriteRenderer.color = new Color(255, 255, 0, 1f);
            Instantiate(Boss.instance.banishEffect, transform.position, Boss.instance.transform.rotation);
            Instantiate(Boss.instance.mob, transform.position, Boss.instance.transform.rotation);
        }
        // bullet impact created otherwise
        else
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            AudioManager.instance.PlaySFX(3);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
