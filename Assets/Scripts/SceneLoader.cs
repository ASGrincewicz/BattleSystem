using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGame(int index)
    {
        var toUnload = SceneManager.GetActiveScene().buildIndex;
        var load = SceneManager.LoadSceneAsync(index);
        if (load.isDone)
            SceneManager.UnloadSceneAsync(toUnload);
        if(Time.timeScale < 1 )
          Time.timeScale = 1;
    }
}
