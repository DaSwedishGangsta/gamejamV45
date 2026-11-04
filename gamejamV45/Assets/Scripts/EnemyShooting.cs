using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class EnemyShooting : MonoBehaviour
{
    public GameObject Bullet;
    private float timer;

    GameObject Target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Target = FindAnyObjectByType<shooting>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 1)
        {
            timer = 0;
            shoot();
        }
    }
    void shoot()
    {
        Vector3 directionToTarget = Target.transform.position - transform.position;
        GameObject newBullet = Instantiate(Bullet, transform.position, Quaternion.identity);
        newBullet.transform.up = directionToTarget;
        
    }
}
