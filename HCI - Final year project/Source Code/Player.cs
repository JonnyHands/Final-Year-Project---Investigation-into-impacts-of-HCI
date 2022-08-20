using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public float moveSpeed;
    private Vector2 moveInput;
    public Rigidbody2D rigidBody;
    public Transform weaponArm;
    private Camera gameCamera;
    public Animator playerAnimator;
    public GameObject bulletToFire;    
    public Transform firePoint;
    public float numberOfShots;
    public float fireRate;
    public int damageToGive;
    private float shotCounter;
    public SpriteRenderer bodySpriteRenderer;
    private float activeMoveSpeed;
    public float dashSpeed = 8f, dashLength = 0.5f, dashCooldown = 2f, dashInvincibility = 0.5f;
    private float dashCounter, dashCoolCounter;
    private bool isDashing;
    public AudioSource walk;
    public bool canMove = true;
    public float spreadAngle = 20f;
    private Quaternion qAngle;
    private Quaternion qDelta;
    public bool endlessUnlocked;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        endlessUnlocked = false;
        gameCamera = Camera.main;
        activeMoveSpeed = moveSpeed;
        numberOfShots = SaveHandler.save.numberOfShots;
        damageToGive = SaveHandler.save.damageToGive;
        fireRate = SaveHandler.save.fireRate;
        UIController.instance.dashSlider.maxValue = dashCooldown;
        UIController.instance.dashSlider.value = dashCoolCounter;
        UIController.instance.dashUI.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        //updates dashUI
        UIController.instance.dashCoolText.text = "Dash ready in: " + dashCoolCounter.ToString("F1") + "s / " + dashCooldown.ToString("F1") +"s";
        UIController.instance.dashSlider.value = dashCoolCounter;
        //checks dash isnt on cooldown
        if(dashCoolCounter > 0)
        {
            UIController.instance.dashUI.SetActive(true);
        }
        else
        {
            UIController.instance.dashUI.SetActive(false);
        }
        //handles player movement, starts by checking if player cna mvoe and game isnt paused
        if (canMove && !LevelManager.instance.isPaused)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();

            rigidBody.velocity = moveInput * activeMoveSpeed;

            Vector3 mousePosition = Input.mousePosition;
            Vector3 cameraPoint = gameCamera.WorldToScreenPoint(transform.localPosition);
            Vector2 offset = new Vector2(mousePosition.x - cameraPoint.x, mousePosition.y - cameraPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            weaponArm.rotation = Quaternion.Euler(0, 0, angle);
            //checks mosue position compared to camera point then rotates player and weapon accordingly
            if (mousePosition.x < cameraPoint.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                weaponArm.localScale = new Vector3(-1f, -1f, 1f);
            }
            //otherwise levels out
            else
            {
                transform.localScale = Vector3.one;
                weaponArm.localScale = Vector3.one;
            }

            qAngle = Quaternion.AngleAxis(-numberOfShots / 2.0f * spreadAngle, firePoint.position) * firePoint.rotation;

            qDelta = Quaternion.AngleAxis(spreadAngle, firePoint.position);

            //checks for LMB being held down
            if (Input.GetMouseButtonDown(0))
            {
                
                shotCounter -= Time.deltaTime;
                //checks tiem left on fire rate
                if (shotCounter <= 0)
                {
                    //method for ability to shoot multiple shots at once
                    for (int i = 0; i < numberOfShots; i++)
                    {
                        if (numberOfShots <= 1)
                        {
                            playerAnimator.SetBool("isShooting", true);
                            Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                            AudioManager.instance.PlaySFX(9);
                            shotCounter = fireRate;
                        }
                        else
                        {
                            playerAnimator.SetBool("isShooting", true);
                            Instantiate(bulletToFire, firePoint.position, qAngle);
                            AudioManager.instance.PlaySFX(9);
                            shotCounter = fireRate;
                            qAngle = qDelta * qAngle;
                        }
                    }
                }
            }
            else
            {
                playerAnimator.SetBool("isShooting", false);
            }
            //checks for LMB single clicks
            if (Input.GetMouseButton(0))
            {
                shotCounter -= Time.deltaTime;

                if (shotCounter <= 0)
                {
                    //method for ability to shoot multiple shots at once
                    for (int i = 0; i < numberOfShots; i++)
                    {
                        if (numberOfShots <= 1)
                        {
                            playerAnimator.SetBool("isShooting", true);
                            Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                            AudioManager.instance.PlaySFX(9);
                            shotCounter = fireRate;
                        }
                        else {
                            playerAnimator.SetBool("isShooting", true);
                            Instantiate(bulletToFire, firePoint.position, qAngle);
                            AudioManager.instance.PlaySFX(9);
                            shotCounter = fireRate;
                            qAngle = qDelta * qAngle;
                        }
                    }
                }

            }
            else
            {
                playerAnimator.SetBool("isShooting", false);
            }
            //checks for space key if hit initiates dash mechanics
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //checks dash is valid to use
                if (dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;
                    playerAnimator.SetTrigger("dash");
                    AudioManager.instance.PlaySFX(6);
                    PlayerHealth.instance.MakeInvincible(dashInvincibility);
                }
            }
            //checks dash coutner
            if (dashCounter > 0)
            {
                
                isDashing = true;
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {

                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCooldown;
                    isDashing = false;
                }
            }
            //checks dashc ooldown
            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }

            //checks if player is moving for animations
            if (moveInput != Vector2.zero)
            {
                playerAnimator.SetBool("isMoving", true);
                if (!walk.isPlaying)
                {
                    walk.Play();
                }
            }
            //cancels move animations
            else
            {
                playerAnimator.SetBool("isMoving", false);
                walk.Stop();
            }
        }
        else
        {
            rigidBody.velocity = Vector2.zero;
            playerAnimator.SetBool("isMoving", false);
        }
    }

    public bool GetIsDashing()
    {
        return isDashing;
    }

    //method for player aniamtion when hitting end of a level
    public void EndLevel()
    {
        playerAnimator.SetTrigger("endLevel");
        walk.Stop();
    }
}
