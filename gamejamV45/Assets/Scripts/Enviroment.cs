using System;
using UnityEditor;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) // bestämer vad den kolliderar med 
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has collided with the environment.");
            
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

    }
    


}
