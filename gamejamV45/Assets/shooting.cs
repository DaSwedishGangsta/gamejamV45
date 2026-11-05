using UnityEngine;

public class shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject Bullet;
    public Transform BulletTransform;
    public bool canFire;
    private float timer;
    public float timeBetweenFiring;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (!canFire )
        {
            timer += Time.deltaTime;
            if(timer > timeBetweenFiring )
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButton(0) && canFire)
        {
            canFire = false;
            Instantiate(Bullet, BulletTransform.position, Quaternion.identity);
        }
    }
}
