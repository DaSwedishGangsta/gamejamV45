using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageOnCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Damage to enemy");
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }
}
