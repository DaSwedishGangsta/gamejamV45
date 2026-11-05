using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

