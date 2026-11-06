using UnityEngine;

public class camerafollow : MonoBehaviour
{
    public float followSpeed = 2f;
    public Transform target;
    [SerializeField] private float yoffset = 2f;
    // Update is called on ce per frame
    void Update() 
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yoffset, -10f); // sätter kamerans position till spelarens position med en offset på y-axeln och en z-axel på -10 för att kameran ska vara framför spelaren.
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime); // slerp gör att kameran följer spelaren med en viss hastighet istället för att bara teleportera till spelarens position.
    }
}
