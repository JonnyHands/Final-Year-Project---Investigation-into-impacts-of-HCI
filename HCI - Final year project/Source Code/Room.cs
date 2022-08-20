using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool closeOnEntry;
    public GameObject[] doors;
    public GameObject mapCover;
    [HideInInspector]
    public bool roomActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //method used to open doors in room
    public void OpenDoors()
    {
        foreach (GameObject d in doors)
        {
            d.SetActive(false);

            closeOnEntry = false;
        }
    }

    //checks for collisions in room activator
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if is with player shuts doors
        if(other.tag == "Player")
        {
            //CameraController.instance.ChangeTarget(transform);
            if (closeOnEntry)
            {
                foreach(GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }
            roomActive = true;
            mapCover.SetActive(false);
        }
    }

    //checks player has left activator to deactivate room
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            roomActive = false;
        }
    }
}
