using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gamekit2D
{
    /// <summary>
    /// This class is used to transition between scenes. This includes triggering all the things that need to happen on transition such as data persistence.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance
        {
            get
            {
                if (instance != null)
                    return instance;

                instance = FindObjectOfType<SceneController>();

                if (instance != null)
                    return instance;

                Create();

                return instance;
            }
        }

        public static bool Transitioning
        {
            get { return Instance.m_Transitioning; }
        }

        protected static SceneController instance;

        public static SceneController Create()
        {
            GameObject sceneControllerGameObject = new GameObject("SceneController");
            instance = sceneControllerGameObject.AddComponent<SceneController>();

            return instance;
        }

        protected string m_CurrentSceneName;
        protected bool m_Transitioning;

        void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        public static void RestartScene()
        {
            Instance.StartCoroutine(Instance.Transition(Instance.m_CurrentSceneName));
        }

        public static void RestartSceneWithDelay(float delay)
        {
            Instance.StartCoroutine(CallWithDelay(delay, RestartScene));
        }

        public static void TransitionToScene(SceneTransitionStart sceneTransitionStart)
        {
            Instance.m_CurrentSceneName = sceneTransitionStart.newSceneName;
            Instance.StartCoroutine(Instance.Transition(sceneTransitionStart.newSceneName));
        }

        protected IEnumerator Transition(string sceneName)
        {
            m_Transitioning = true;

            yield return StartCoroutine(SceneFader.FadeSceneOut(SceneFader.FadeType.Loading));

            if (sceneName != null)
            {
                yield return SceneManager.LoadSceneAsync(sceneName);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("No scene transition start was found");
#endif
                yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            }

            SceneTransitionEnd sceneTransitionEnd = GetSceneTransitionEnd();
            if (sceneTransitionEnd != null)
            {
                SetupNewScene(sceneTransitionEnd);
            }
            yield return StartCoroutine(SceneFader.FadeSceneIn());

            m_Transitioning = false;
        }

        protected SceneTransitionEnd GetSceneTransitionEnd()
        {
            SceneTransitionEnd sceneTransitionEnd = FindObjectOfType<SceneTransitionEnd>();
            if (sceneTransitionEnd != null)
            {
                return sceneTransitionEnd;
            }
#if UNITY_EDITOR
            Debug.LogWarning("No scene transition end was found");
#endif
            return null;
        }

        protected void SetupNewScene(SceneTransitionEnd sceneTransitionEnd)
        {

        }

        static IEnumerator CallWithDelay(float delay, Action call)
        {
            yield return new WaitForSeconds(delay);
            call();
        }
    }
}