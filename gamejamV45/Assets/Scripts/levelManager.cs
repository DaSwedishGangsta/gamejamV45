using UnityEngine;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) // bestämer vad den kolliderar med för att betya level
    {
        if (collision.gameObject.CompareTag("LevelChange"))
        {
            Debug.Log("Player has collided with the level manager.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // laddar nästa level
        }
    }
}
