using System;
using UnityEditor;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has collided with the environment.");
            
        }
        
    }


}
