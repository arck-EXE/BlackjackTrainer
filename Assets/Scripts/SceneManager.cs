using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void SceneChangeGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainWindow");
    }
}
