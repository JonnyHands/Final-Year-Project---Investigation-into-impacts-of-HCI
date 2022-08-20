using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSort : MonoBehaviour
{

    private SpriteRenderer sr;
    // method used to order sprites so the layering looks as expected with graves and pieces.
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
