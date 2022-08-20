using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveSmash : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 direction;
    public float decelerate = 5f;
    public float appearanceLength = 3f;
    public SpriteRenderer sr;
    public float fadeTime = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        //selects random direction for pieces to travel in for x and y direction
        direction.x = Random.Range(-speed, speed);
        direction.y = Random.Range(-speed, speed);
    }

    // Update is called once per frame
    void Update()
    {
        //method to make pieces move at a speed and slow down to a stop
        transform.position += direction * Time.deltaTime;
        direction = Vector3.Lerp(direction, Vector3.zero, decelerate * Time.deltaTime);
        appearanceLength -= Time.deltaTime;

        //checks time left to be visible
        if (appearanceLength < 0)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.MoveTowards(sr.color.a, 0f, fadeTime*Time.deltaTime));

            if(sr.color.a == 0f)
            {
                Destroy(gameObject);
            }
            
        }
    }
}
