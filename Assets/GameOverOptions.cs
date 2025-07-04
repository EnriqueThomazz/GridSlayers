using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverOptions : MonoBehaviour
{
    public void gotoMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
