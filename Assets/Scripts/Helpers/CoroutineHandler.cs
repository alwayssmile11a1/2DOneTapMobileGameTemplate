using UnityEngine;
using System.Collections;

namespace Gamekit2D
{
    /// <summary>
    /// This class allows us to start Coroutines from non-Monobehaviour scripts
    /// Create a GameObject it will use to launch the coroutine on
    /// </summary>
    public class CoroutineHandler : MonoBehaviour
    {
        static protected CoroutineHandler m_Instance;
        static public CoroutineHandler instance
        {
            get
            {
                if (m_Instance == null)
                {
                    GameObject newGameObject = new GameObject("CoroutineHandler");
                    //DontDestroyOnLoad(newGameObject);
                    m_Instance = newGameObject.AddComponent<CoroutineHandler>();
                }

                return m_Instance;
            }
        }

        public void OnDisable()
        {
            if (m_Instance)
                Destroy(m_Instance.gameObject);
        }

        static public Coroutine StartStaticCoroutine(IEnumerator coroutine)
        {
            return instance.StartCoroutine(coroutine);
        }
    }
}