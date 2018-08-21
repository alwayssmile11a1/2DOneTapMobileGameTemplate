using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gamekit2D
{
    public class SceneTransitionStart : MonoBehaviour
    {
        [SceneName]
        public string newSceneName;

        #region temporarily deleted
        //public enum TransitionType
        //{
        //    DifferentScene, SameScene
        //}


        //public enum TransitionWhen
        //{
        //    ExternalCall, OnTriggerEnter
        //}

        //[Tooltip("This is the gameobject that will transition if the transitionwhen is set to OnTriggerEnter. For example, the player.")]
        //public GameObject transitioningGameObject;

        //[Tooltip("Whether the transition will be within this scene, to a different zone or a non-gameplay scene.")]
        //public TransitionType transitionType;

        //[Tooltip("What should trigger the transition to start.")]
        //public TransitionWhen transitionWhen;


        //bool m_TransitioningGameObjectPresent;

        //void Start()
        //{
        //if (transitionWhen == TransitionWhen.ExternalCall)
        //    m_TransitioningGameObjectPresent = true;
        //}

        //void OnTriggerEnter2D(Collider2D other)
        //{
        //    if (other.gameObject == transitioningGameObject)
        //    {
        //        m_TransitioningGameObjectPresent = true;

        //        if (SceneFader.IsFading || SceneController.Transitioning)
        //            return;

        //        if (transitionWhen == TransitionWhen.OnTriggerEnter)
        //            TransitionInternal();
        //    }
        //}

        //void OnTriggerExit2D(Collider2D other)
        //{
        //    if (other.gameObject == transitioningGameObject)
        //    {
        //        m_TransitioningGameObjectPresent = false;
        //    }
        //}
        #endregion

        protected void TransitionInternal()
        {
            SceneController.TransitionToScene(this);
        }

        public void Transition()
        {
            TransitionInternal();
        }
    }
}