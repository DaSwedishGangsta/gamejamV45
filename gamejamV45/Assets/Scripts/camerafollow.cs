using UnityEngine;

public class camerafollow : MonoBehaviour
{
    public float followSpeed = 2f;
    public Transform target;

    // Update is called on ce per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }
}
