using System;
using UnityEngine;

namespace Gamekit2D
{
    public class SceneControllerWrapper : MonoBehaviour
    {
        public void RestartScene()
        {
            SceneController.RestartScene();
        }

        public void TransitionToScene(SceneTransitionStart transitionPoint)
        {
            SceneController.TransitionToScene(transitionPoint);
        }

        public void RestartSceneWithDelay(float delay)
        {
            SceneController.RestartSceneWithDelay(delay);
        }
    }
}