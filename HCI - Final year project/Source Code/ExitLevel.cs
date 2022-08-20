using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ExitLevel : MonoBehaviour
{
    public string levelToLoad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //checks if collides with player if so runs end level methods and initiates coroutine
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player.instance.EndLevel();
            StartCoroutine(LevelManager.instance.LevelEnd());
        }
    }
}
