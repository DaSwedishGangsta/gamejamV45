using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    public GameObject Enemy;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float force;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    // Update is called once per frame
    private void Update()
    {
        StartCoroutine(Bulletdestroy());
    }

    private IEnumerator Bulletdestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
