using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D enemyRigidBody;
    public float speed;
    //shoot mechanic
    public bool shoots;
    public GameObject shot;
    public Transform shotStart;
    public float fireSpeed;
    private float shotTimer;
    //run mechanic
    public bool cowers;
    public float runAwayRange;
    //chase mechanic
    public bool chases;
    public float rangeOfAgro;
    public float agroRange;
    //roam mechanic
    public bool roams;
    public float roamTime;
    public float pauseTime;
    private float roamCount;
    private float pauseCount;
    private Vector3 roamDirection;
    //slime mechanic
    public GameObject tinySlime;
    public GameObject smallSlime;
    public bool isSlime;
    public bool isSmallSlime;
    private Vector3 moveDirection;    
    //drop items
    public bool dropItem;
    public GameObject[] items;
    public float dropPercent;
    //defaults
    public Animator skeletonAnimator;
    public int health = 50;
    public GameObject banishEffect;     
    public SpriteRenderer body;
    //Health
    public Slider healthSlider;
    //public Text healthText;


    // Start is called before the first frame update
    void Start()
    {
        //checks if enemy should roam if so initiates variables randomly for its roaming
        if (roams)
        {
            pauseCount = Random.Range(pauseTime * .5f, pauseTime * 1.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = Vector3.zero;
        
        //checks if is visible and if player is active
        if (body.isVisible && Player.instance.gameObject.activeInHierarchy)
        {
            //checks if player is within range of aggresion zone if so moves towards player
            if (Vector3.Distance(transform.position, Player.instance.transform.position) < rangeOfAgro && chases)
            {
                moveDirection = Player.instance.transform.position - transform.position;
            }
            else
            {
                //checks if the enemy has a roam mechanic if so enemy will move randomly stop move randomly stop, on repeat until it aggros
                if (roams)
                {
                    if (roamCount > 0)
                    {
                        roamCount -= Time.deltaTime;
                        moveDirection = roamDirection;
                        if (roamCount <= 0)
                        {
                            pauseCount = Random.Range(pauseTime * .5f, pauseTime * 1.5f);
                        }
                    }
                    if (pauseCount > 0)
                    {
                        pauseCount -= Time.deltaTime;
                        if (pauseCount < -0)
                        {
                            roamCount  = Random.Range(roamTime * .5f, roamTime * 1.5f);
                            roamDirection = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
                        }
                    }
                }


            }
            //checks if enemy cowers, if so when player is within aggression range enemy runs opposite direction
            if(cowers && Vector3.Distance(transform.position, Player.instance.transform.position) < runAwayRange)
            {
                moveDirection = transform.position - Player.instance.transform.position - transform.position;
                
            }
            
            

            moveDirection.Normalize();
            enemyRigidBody.velocity = moveDirection * speed;

            //checks if enemy shoots, if so enemy will shoot when player is within range
            if (shoots && Vector3.Distance(transform.position, Player.instance.transform.position) < agroRange)
            {
                shotTimer -= Time.deltaTime;
                if (shotTimer <= 0)
                {
                    shotTimer = fireSpeed;
                    Instantiate(shot, shotStart.transform.position, transform.rotation);
                    AudioManager.instance.PlaySFX(10);
                }
            }
        }
        else
        {
            enemyRigidBody.velocity = Vector2.zero;
        }
        if (moveDirection != Vector3.zero)
        {
            skeletonAnimator.SetBool("isMoving", true);
        }
        else
        {
            skeletonAnimator.SetBool("isMoving", false);
        }

    }

    //method to damage the enemy
    public void DamageEnemy(int damage)
    {
        health -= damage;

        healthSlider.value = health;

        AudioManager.instance.PlaySFX(1);
        //checks if enemy should die
        if (health <= 0)
        {
            //checks if enemy drops items
            if (dropItem)
            {
                //rolls a chance to drop items
                float dropChance = Random.Range(0f, 100f);
                if (dropPercent > dropChance)
                {
                    int randomItem = Random.Range(0, items.Length);
                    Instantiate(items[randomItem], transform.position, transform.rotation);
                }
            }
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(0);
            Instantiate(banishEffect, transform.position, transform.rotation);
            float x;
            float y;
            float z;
            x = Random.Range(-.3f, .3f);
            y = Random.Range(-.3f, .3f);
            z = Random.Range(-.3f, .3f);
            Vector3 randomSpawn = new Vector3(x, y, z);
            //checks if slime is so spawns 3 small slimes
            if (isSlime)
            {
                Instantiate(smallSlime, transform.position += randomSpawn, transform.rotation);
                Instantiate(smallSlime, transform.position += randomSpawn, transform.rotation);
                Instantiate(smallSlime, transform.position += randomSpawn, transform.rotation);
            }
            //checks if small slime if so spawns 3 slimes
            if (isSmallSlime)
            {
                Instantiate(tinySlime, transform.position += randomSpawn, transform.rotation);
                Instantiate(tinySlime, transform.position += randomSpawn, transform.rotation);
                Instantiate(tinySlime, transform.position += randomSpawn, transform.rotation);
            }
        }
    }
}
