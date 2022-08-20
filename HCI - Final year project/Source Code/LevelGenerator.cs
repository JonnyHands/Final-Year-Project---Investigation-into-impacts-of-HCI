using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;
    public int distanceToEnd;
    public Color start, last, vendor, damageVendor;
    public Transform generationPoint;
    public enum Direction { up, down, left, right };
    public Direction selectedDirection;
    public float offsetX = 18f;
    public float offsetY = 10f;
    public LayerMask isRoom;
    private GameObject finalRoom, vendorRoom, damageVendorRoom;
    private List<GameObject> layoutRoomObjects = new List<GameObject>();
    public RoomPrefabs rooms;
    private List<GameObject> generatedOutlines = new List<GameObject>();
    public RoomCenter startCenter, endCenter, vendorCenter, damageVendorCenter;
    public RoomCenter[] centers;
    [Header("Vendor settings")]
    public int minVendor;
    public int maxVendor;
    public bool needVendor;
    [Header("Damage Vendor settings")]
    public int minDamageVendor;
    public int maxDamageVendor;
    public bool needDamageVendor;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(layoutRoom, generationPoint.position, generationPoint.rotation).GetComponent<SpriteRenderer>().color = start;

        selectedDirection = (Direction)Random.Range(0, 4);
        moveGenPoint();
        //creates a random room for every slot to the final room ensuring there is a final room and starting room set with two shops
        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generationPoint.position, generationPoint.rotation);
            layoutRoomObjects.Add(newRoom);
            if (i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = last;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                finalRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            moveGenPoint();

            while (Physics2D.OverlapCircle(generationPoint.position, .2f, isRoom))
            {
                moveGenPoint();
            }          

        }
        //check for if shop is needed
        if (needVendor)
        {
            int selectVendor = Random.Range(minVendor, maxVendor);
            vendorRoom = layoutRoomObjects[selectVendor];
            layoutRoomObjects.RemoveAt(selectVendor);
            vendorRoom.GetComponent<SpriteRenderer>().color = vendor;
        }
        //check for if damage shop is needed
        if (needDamageVendor)
        {
            int selectDamageVendor = Random.Range(minDamageVendor, maxDamageVendor);
            damageVendorRoom = layoutRoomObjects[selectDamageVendor];
            layoutRoomObjects.RemoveAt(selectDamageVendor);
            damageVendorRoom.GetComponent<SpriteRenderer>().color = damageVendor;
        }

        //room outlines created to format structure of map and implement physics
        CreateRoomOutline(Vector3.zero);
        foreach (GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }
        CreateRoomOutline(finalRoom.transform.position);
        if (needVendor)
        {
            CreateRoomOutline(vendorRoom.transform.position);
        }
        if (needDamageVendor)
        {
            CreateRoomOutline(damageVendorRoom.transform.position);
        }

        //outlines are generated to match room centers
        foreach (GameObject room in generatedOutlines)
        {
            bool generateCenter = true;
            if(room.transform.position == Vector3.zero)
            {
                Instantiate(startCenter, room.transform.position, transform.rotation).theRoom = room.GetComponent<Room>();
                generateCenter = false;
            }
            if (room.transform.position == finalRoom.transform.position)
            {
                Instantiate(endCenter, room.transform.position, transform.rotation).theRoom = room.GetComponent<Room>();
                generateCenter = false;
            }
            if (needVendor)
            {
                if (room.transform.position == vendorRoom.transform.position)
                {
                    Instantiate(vendorCenter, room.transform.position, transform.rotation).theRoom = room.GetComponent<Room>();
                    generateCenter = false;
                }
            }
            if (needDamageVendor)
            {
                if (room.transform.position == damageVendorRoom.transform.position)
                {
                    Instantiate(damageVendorCenter, room.transform.position, transform.rotation).theRoom = room.GetComponent<Room>();
                    generateCenter = false;
                }
            }
            if (generateCenter)
            {
                int centerSelect = Random.Range(0, centers.Length);
                Instantiate(centers[centerSelect], room.transform.position, transform.rotation).theRoom = room.GetComponent<Room>();
            }
            
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    //callibrates the centers to the outlines so they matchup and there is no overlaps
    public void moveGenPoint()
    {
        switch (selectedDirection)
        {
            case Direction.up:
                generationPoint.position += new Vector3(0f, offsetY, 0f);
                break;
            case Direction.down:
                generationPoint.position += new Vector3(0f, -offsetY, 0f);
                break;
            case Direction.right:
                generationPoint.position += new Vector3(offsetX, 0f, 0f);
                break;
            case Direction.left:
                generationPoint.position += new Vector3(-offsetX, 0f, 0f);
                break;
        }
    }


    //method that creates the outlines and determiens direction of next room randomly
    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, offsetY, 0f), .2f, isRoom);
        bool roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -offsetY, 0f), .2f, isRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-offsetX, 0f, 0f), .2f, isRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(offsetX, 0f, 0f), .2f, isRoom);

        int directionCount = 0;
        if (roomUp)
        {
            directionCount++;
        }
        if (roomDown)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }

        switch (directionCount)
        {
            case 0:
                Debug.LogError("No room exists");
                break;
            case 1:

                if (roomUp)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleU, roomPosition, transform.rotation));
                }

                if (roomDown)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleD, roomPosition, transform.rotation));
                }

                if (roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleL, roomPosition, transform.rotation));
                }

                if (roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleR, roomPosition, transform.rotation));
                }

                break;

            case 2:

                if (roomUp && roomDown)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUD, roomPosition, transform.rotation));
                }

                if (roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLR, roomPosition, transform.rotation));
                }

                if (roomUp && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUR, roomPosition, transform.rotation));
                }

                if (roomRight && roomDown)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleRD, roomPosition, transform.rotation));
                }

                if (roomDown && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLD, roomPosition, transform.rotation));
                }

                if (roomLeft && roomUp)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLU, roomPosition, transform.rotation));
                }

                break;

            case 3:

                if (roomUp && roomRight && roomDown)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleURD, roomPosition, transform.rotation));
                }

                if (roomRight && roomDown && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleLRD, roomPosition, transform.rotation));
                }

                if (roomDown && roomLeft && roomUp)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleLUD, roomPosition, transform.rotation));
                }

                if (roomLeft && roomUp && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleLUR, roomPosition, transform.rotation));
                }

                break;

            case 4:


                if (roomDown && roomLeft && roomUp && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.quadLURD, roomPosition, transform.rotation));
                }

                break;
        }
    }
}

//class to store prefabs of different room variants
    [System.Serializable]
    public class RoomPrefabs
    {
        public GameObject singleR, singleU, singleL, singleD,
            doubleLR, doubleUD, doubleUR, doubleRD, doubleLD, doubleLU,
            tripleURD, tripleLRD, tripleLUD, tripleLUR,
            quadLURD;
    }
