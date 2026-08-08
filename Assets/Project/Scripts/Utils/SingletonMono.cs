using UnityEngine;

public abstract class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
        private static T _instance;
        public static T instance => _instance;

        protected virtual void Awake()
        {
                if(_instance == null) _instance = this as T;
                else Destroy(this);
        }

        protected virtual void OnDestroy()
        {
                if (Equals(_instance)) _instance = null;
        }
}