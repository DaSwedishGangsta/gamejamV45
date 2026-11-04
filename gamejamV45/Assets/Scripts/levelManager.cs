using UnityEngine;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LevelChange"))
        {
            Debug.Log("Player has collided with the level manager.");
            SceneManager.LoadScene("level2");
        }
    }
}
