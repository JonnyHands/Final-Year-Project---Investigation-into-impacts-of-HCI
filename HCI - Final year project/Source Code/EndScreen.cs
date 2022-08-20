using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public float waitToDisplay = 2f;
    public GameObject anyKey;
    public string mainMenuScene;
    public bool endlessUnlocked;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        endlessUnlocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        //checks if display weight timer is more than 0
        if (waitToDisplay > 0)
        {
            //takes time off counter
            waitToDisplay -= Time.deltaTime;
            //if counter hits 0 prompt to hit key to continue displays
            if (waitToDisplay <= 0)
            {
                anyKey.SetActive(true);
                Player.instance.endlessUnlocked = true;
            }
        }
        else
        {
            //check for key input, if found loads next scene.
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(mainMenuScene);
            }
        }
        
    }
}
