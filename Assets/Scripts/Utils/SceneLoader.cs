using UnityEngine;
using UnityEngine.SceneManagement;
using Veganimus.BattleSystem;

[CreateAssetMenu(menuName = "Scene Loader")]
public class SceneLoader : ScriptableObject
{ 
    public int sceneIndex;

    protected void Awake()
    {
    }
    public void LoadGame(int index)
    {
        sceneIndex = index;
        var toUnload = SceneManager.GetActiveScene().buildIndex;
        var load = SceneManager.LoadSceneAsync(3);
        if (load.isDone)
        {
            SceneManager.UnloadSceneAsync(toUnload);
        }
        if(Time.timeScale < 1 )
          Time.timeScale = 1;
    }
}
