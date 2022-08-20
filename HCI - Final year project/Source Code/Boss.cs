using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

    
    public BossIteration[] iteration;
    private int currentIteration;
    private float iterationCounter;
    private float shotCounter;
    public Rigidbody2D rigidBody;
    private Vector2 direction;
    public Animator bossAnimator;
    public AudioSource laugh;
    public int currentHealth;
    public GameObject banishEffect;
    public GameObject endGame;
    public Slider healthSlider;
    public BossStage[] tiers;
    public int currentTier;
    public SpriteRenderer bodySpriteRenderer;
    public SpriteRenderer larmSpriteRenderer;
    public SpriteRenderer rarmSpriteRenderer;
    public float invincibilityLength = 1f;
    private float invincibilityCounter;
    public bool isInvincible;
    public GameObject mob;
    public GameObject music;
    public static Boss instance;
    // Start is called before the first frame update

    private void Awake()
    {
        AudioManager.instance.PlayBossMusic();
        instance = this;
    }
    void Start()
    {
        AudioManager.instance.PlayBossMusic();
        iteration = tiers[currentTier].iterations;
        iterationCounter = iteration[currentIteration].length;
        UIController.instance.bossHealth.SetActive(true);
        currentHealth = Player.instance.damageToGive * 50;
        healthSlider.maxValue = currentHealth;
        tiers[0].stagehealthThreshold = Player.instance.damageToGive * 50 / 2;
    }

    // Update is called once per frame
    void Update()
    {
        //checks if is invincible
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
            //if invincible changes colour for feedback
            if (invincibilityCounter <= 0)
            {
                isInvincible = false;
                bodySpriteRenderer.color = new Color(255, 255, 255, 0.75f);
                rarmSpriteRenderer.color = new Color(255, 255, 255, 0.75f);
                larmSpriteRenderer.color = new Color(255, 255, 255, 0.75f);
            }
        }
        healthSlider.value = currentHealth;

        if (iterationCounter > 0)
        {
            iterationCounter -= Time.deltaTime;
            direction = Vector2.zero;
            //checks if should move if so does actions accordingly
            if (iteration[currentIteration].shouldMove)
            {
                AudioManager.instance.PlaySFX(16);
                bossAnimator.SetBool("isMoving", true);
                //checks if should chase player if so does so
                if (iteration[currentIteration].shouldChase)
                {
                    if (Player.instance.transform.position.x > transform.position.x)
                    {
                        transform.localScale = new Vector3(-1f, 1f, 1f);
                    }
                    else
                    {
                        transform.localScale = Vector3.one;
                    }

                    direction = Player.instance.transform.position - transform.position;
                    direction.Normalize();
                }
                //checks if should be moving from position 
                if (iteration[currentIteration].moveToLocation)
                {
                    direction = iteration[currentIteration].locationPoints.position - transform.position;                    
                }
            }
            else
            {
                bossAnimator.SetBool("isMoving", false);
            }

            rigidBody.velocity = direction * iteration[currentIteration].moveSpeed;

            //checks if boss should be attacking
            if (iteration[currentIteration].shouldShoot)
            {
                
                bossAnimator.SetBool("isShooting", true);
                bodySpriteRenderer.color = new Color(0, 255, 0, 1f);
                larmSpriteRenderer.color = new Color(0, 255, 0, 1f);
                rarmSpriteRenderer.color = new Color(0, 255, 0, 1f);
                invincibilityCounter = invincibilityLength;
                isInvincible = true;
                shotCounter -= Time.deltaTime;
                if (shotCounter <= 0)
                {
                    
                    shotCounter = iteration[currentIteration].timeBetweenShots;
                    foreach (Transform t in iteration[currentIteration].shotPoints)
                    {
                        if (transform.localScale == Vector3.one)
                        {
                            AudioManager.instance.PlaySFX(2);
                            Instantiate(iteration[currentIteration].bullet, t.position, t.rotation);
                        }
                        else
                        {
                            Instantiate(iteration[currentIteration].bullet,  t.position, t.rotation * Quaternion.Euler(0f, 180f, 0f));
                        }

                    }
                }
            }
            else
            {
                bossAnimator.SetBool("isShooting", false);
            }
        }
        else
        {
            currentIteration++;
            if (currentIteration >= iteration.Length)
            {
                currentIteration = 0;
            }
            iterationCounter = iteration[currentIteration].length;
        }
    }

    //method for visual feedback when boss takes damage and to remove health
    public void TakeDamage(int damageAmount)
    {
        if (invincibilityCounter <= 0)
        {
            bodySpriteRenderer.color = new Color(255, 0, 0, 0.5f);
            larmSpriteRenderer.color = new Color(255, 0, 0, 0.5f);
            rarmSpriteRenderer.color = new Color(255, 0, 0, 0.5f);
            invincibilityCounter = invincibilityLength;
            isInvincible = true;
            currentHealth -= damageAmount;
            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);
                Instantiate(banishEffect, transform.position, transform.rotation);
                endGame.transform.position = transform.position;
                UIController.instance.bossHealth.SetActive(false);
                endGame.SetActive(true);

            }
            else
            {
                if (currentHealth <= tiers[currentTier].stagehealthThreshold && currentTier + 1 < tiers.Length)
                {
                    currentTier++;
                    iteration = tiers[currentTier].iterations;
                    currentIteration = 0;
                    iterationCounter = iteration[currentIteration].length;
                }
            }
        }
    }

    //method to clear scene if player dies
    public void PlayerDied()
    {
        gameObject.SetActive(false);
        music.SetActive(false);

    }

    //check to make damage when colliding with player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !PlayerHealth.instance.GetIsInvincible())
        {
            PlayerHealth.instance.DamagePlayer(2);
        }

    }
}


//seperate class to handle boss iterations
    [System.Serializable]
    public class BossIteration
    {
        public float length;
        public bool shouldMove;
        public bool shouldChase;
        public float moveSpeed;
        public bool moveToLocation;
        public Transform locationPoints;
        public bool shouldShoot;
        public GameObject bullet;
        public float timeBetweenShots;
        public Transform[] shotPoints;
    }

[System.Serializable]
public class BossStage
{
    //stores iterations and manages threshold for rotations
    public BossIteration[] iterations;
    public int stagehealthThreshold;

    void Start()
    {
        
    }
}
