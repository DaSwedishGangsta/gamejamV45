using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class EnemyShooting : MonoBehaviour
{
    public GameObject Bullet;
    public GameObject Player = null;
    public bool Aggroed = false;
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
        if (Aggroed == false)
        {
            float aggroDistance = 12.0f;
            var distanceBetween = Player.transform.position - transform.position;
            if (distanceBetween.magnitude < aggroDistance)
            {
                Aggroed = true;
                Debug.Log("active");
            }
            else
            {
                return;
                Debug.Log("Not active");
            }
        }
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
