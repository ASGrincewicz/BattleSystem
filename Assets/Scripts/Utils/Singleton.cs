using UnityEngine;
namespace Veganimus.BattleSystem
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("Class is NULL!");
                }
                return _instance;
            }
        }
        protected virtual void Awake() => _instance = this as T;
    }
}
