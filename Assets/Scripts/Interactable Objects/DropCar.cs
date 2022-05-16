using System.Collections;
using UnityEngine;

namespace Interactable_Objects
{
    public class DropCar : EventInteractable
    {
        #region Inspector

        [Header("Sound Interaction")]
        [SerializeField]
        private AudioSource regularClip;

        [SerializeField]
        private AudioSource dropClip;

        [SerializeField]
        private float animationDelay;

        #endregion

        #region Private Fields

        private bool firstPlay;
        private Animator carAnimator;

        #endregion

        #region MonoBehaviour

        void Start()
        {
            carAnimator = GetComponentInParent<Animator>();
        }

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.Space))
        //     {
        //         DropSound();
        //     }
        // }

        #endregion


        #region Coroutines

        /// <summary>
        /// This Enumerator will start playing the default sound again.
        /// </summary>
        private IEnumerator RegularSound()
        {
            yield return new WaitForSeconds(dropClip.clip.length + 2f);
            regularClip.loop = true;
            carAnimator.enabled = false;
            regularClip.Play();
        }

        /// <summary>
        /// This Enumerator will start the animation.
        /// </summary>
        private IEnumerator StartAnimation()
        {
            yield return new WaitForSeconds(animationDelay);
            carAnimator.enabled = true;
        }

        #endregion

        #region EventInteractable

        protected override void ScriptInteract()
        {
            if (!dropClip.isPlaying)
            {
                regularClip.loop = false;
                dropClip.PlayDelayed(+regularClip.clip.length - regularClip.time - 0.1f);
                StartCoroutine(RegularSound());
                StartCoroutine(StartAnimation());
            }
        }

        #endregion
    }
}