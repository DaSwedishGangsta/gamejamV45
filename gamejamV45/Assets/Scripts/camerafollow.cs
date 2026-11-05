using UnityEngine;

public class camerafollow : MonoBehaviour
{
    public float followSpeed = 2f;
    public Transform target;
    [SerializeField] private float yoffset = 2f;
    // Update is called on ce per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yoffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }
}
