using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LookAtTarget : MonoBehaviour
{
    public GameObject Target = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            transform.position += new Vector3(0, 1f, 0)* Time.deltaTime;
        }
        
        Vector3 directionToTarget = Target.transform.position - transform.position;

        // Calculate rotation that points +Z at target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        // Rotate -90 degrees around Y so local +X points forward instead
        transform.rotation = targetRotation * Quaternion.Euler(0, -90f, 90f);
    }
}
