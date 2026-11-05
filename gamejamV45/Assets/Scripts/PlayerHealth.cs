using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxhealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxhealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
