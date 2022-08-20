using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    public bool openWhenWiped;
    public List<GameObject> mobsLeft = new List<GameObject>();
    public Room theRoom;

    // Start is called before the first frame update
    void Start()
    {
        if (openWhenWiped)
        {
            theRoom.closeOnEntry = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //checks mob coutn, if mob count is 0 doors will open
        if (mobsLeft.Count > 0 && theRoom.roomActive && openWhenWiped)
        {
            for (int i = 0; i < mobsLeft.Count; i++)
            {
                if (mobsLeft[i] == null)
                {
                    mobsLeft.RemoveAt(i);
                    i--;
                }
            }
            if (mobsLeft.Count == 0)
            {
                theRoom.OpenDoors();
            }
        }
    }
}
