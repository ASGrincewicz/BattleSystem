using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
namespace Veganimus.BattleSystem
{
    public class Loading : Singleton<Loading>
    {
        public SceneLoader loader;
        [SerializeField] private Image _progessBar;
        [SerializeField] private TMP_Text _loadingProgress;
        public int sceneIndex;

        protected override void Awake() => _instance = this;

        private void Start() => StartCoroutine(LoadLevelASync());
      

        public IEnumerator LoadLevelASync()
        {
            sceneIndex = loader.sceneIndex;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
           
            while (asyncOperation.isDone == false)
            {
                _progessBar.fillAmount = asyncOperation.progress;
                _loadingProgress.text = $"Loading: {asyncOperation.progress * 100}%";
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
