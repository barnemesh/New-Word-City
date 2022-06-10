using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Interactable_Objects
{
    public class Shark : EventInteractable
    {
        #region Inspector

        [SerializeField] 
        private Animator tubeAnimator;

        [SerializeField]
        [Tooltip("First sprite is when not close, Second sprite is when close")]
        private Sprite[] tubeSprites;

        [SerializeField] 
        [Tooltip("SpriteRenderer of tube that player should be close to so interaction can be made")]
        private SpriteRenderer tubeRenderer;


        #endregion
        
        #region Private Fields
        
        private Animator _sharkAnimator;

        private Animator _tubeAnimator;
        
        private Beach _beach;

        #endregion
        
        
        #region EventInteractable

        private void Start()
        {
            _sharkAnimator = GetComponent<Animator>();
            _beach = FindObjectOfType<Beach>();
        }

        #endregion


        protected override void ScriptInteract()
        {
            _sharkAnimator.SetTrigger("Movement");
            _sharkAnimator.SetBool("Animation", true);
            _beach.ChangeSound(1);
        }


        public void StopInteraction()
        {
            _beach.ChangeSound(0);
            _sharkAnimator.SetBool("Animation", false);
        }


        public void CloseToTube(int sprite)
        {
            tubeRenderer.sprite = tubeSprites[sprite];
        }

    }
}

