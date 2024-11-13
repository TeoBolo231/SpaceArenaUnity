using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
   
    public float speedPlatform = 2f;
    public int right = 3;
    public int left = -3;
    
    bool moveRight = true;

    void Update()
    {
        if(transform.position.x > right)
        {
            moveRight = false;
        }

        if (transform.position.x < left)
        {
            moveRight = true;
        }

        if(moveRight)
        {
            transform.position = new Vector2(transform.position.x + speedPlatform * Time.deltaTime, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - speedPlatform * Time.deltaTime, transform.position.y);
        }
    }

}
