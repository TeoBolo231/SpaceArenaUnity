using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDestroyLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) //destroys bullets on impact with the environment
    {
        Bullet bullet = other.gameObject.GetComponent<Bullet>();

        BulletManager(bullet);
    }

    private void BulletManager(Bullet bullet)
    {
        bullet.Hit(); 
    }
    
}
