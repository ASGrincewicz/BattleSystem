using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGame(int index)
    {
        var toUnload = SceneManager.GetActiveScene().buildIndex;
        var load = SceneManager.LoadSceneAsync(1);
        if (load.isDone)
            SceneManager.UnloadSceneAsync(toUnload);
    }
}
