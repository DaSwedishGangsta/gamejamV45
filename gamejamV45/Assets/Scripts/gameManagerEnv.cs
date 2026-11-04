using Unity.VisualScripting;
using UnityEngine;

public class gameManagerEnv : MonoBehaviour
{
    public static gameManagerEnv Instance;
    public GameObject hazard;
    public GameObject hazardClone;
    public float timer;
    [SerializeField] private float spawnInterval = 5f; 
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            hazardClone = Instantiate(hazard, new Vector3(2, 2, 0), transform.rotation) as GameObject;
            timer = 0;
        }


    }
    
}
