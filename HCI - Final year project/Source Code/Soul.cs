using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    public int value = 1;
    public Transform soul;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //method to add spinning affect to souls
    private void FixedUpdate()
    {
        transform.Rotate(0, 10, 0 * Time.deltaTime);
    }

    //method to check if player has touched soul if so adds to soul count and plays feedback
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioManager.instance.PlaySFX(4);
            LevelManager.instance.ReceiveSouls(value);
            Destroy(gameObject);
        }
    }
}
