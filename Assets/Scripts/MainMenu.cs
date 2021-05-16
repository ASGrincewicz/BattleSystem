using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        var load = SceneManager.LoadSceneAsync(1);
        if (load.isDone)
            SceneManager.UnloadSceneAsync(0);
    }

    public void LinkedInButton() => Application.OpenURL("http://www.linkedin.com/in/aarongrincewicz");
}