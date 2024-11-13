using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour  
{
    [SerializeField] int bulletDamage = 10;

    [SerializeField] public GameObject hitEffect;
    
    private Collider2D myCollider2D;

    public int GetDamage()
    {
        return bulletDamage;
    }

    public void Hit()
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        Destroy(gameObject);
    }
}
