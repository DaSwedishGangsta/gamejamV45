using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Rigidbody2D rb;
    public float force = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * force;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
