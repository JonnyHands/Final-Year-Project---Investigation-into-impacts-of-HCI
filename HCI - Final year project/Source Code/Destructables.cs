using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructables : MonoBehaviour
{
    public GameObject[] pieces;
    public int maxPieces = 5;
    public bool dropItem;
    public GameObject[] items;
    public float dropChance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //checks for collisions
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if with player and dashing or by bullet, grave smashes
        if(other.tag == "Player" && Player.instance.GetIsDashing() || other.tag == "Player Bullet")
        {
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(14);
            int piecesToDrop = Random.Range(1, maxPieces);

            //effect to make pieces scatter
            for(int i = 0; i < piecesToDrop; i++)
            {
                int randomPiece = Random.Range(0, pieces.Length);
                int rotation = Random.Range(0, 4);
                
                Instantiate(pieces[randomPiece], transform.position, Quaternion.Euler(0, 0, rotation * 90));
            }
            //method to spawn pickups
            if (dropItem)
            {
                float dropChance = Random.Range(0f, 100f);
                if (this.dropChance > dropChance)
                {
                    int randomItem = Random.Range(0, items.Length);
                    Instantiate(items[randomItem], transform.position, transform.rotation);
                }
            }
        }
    }
}
