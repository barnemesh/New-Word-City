using System;
using System.Collections;
using System.Collections.Generic;
using Player_Control;
using UnityEngine;

namespace Interactable_Objects
{
    public class Trampolina : PairedAnimationInteractable
    {
        /// @see PairedAnimationInteractable.cs
        [Space]
        [Header("Trampoline Interactable")]
        [SerializeField]
        [Tooltip("How much the circle will be close between tutorial-world transition")]
        private float duration;

        [SerializeField]
        private Vector3 jumpNewPosition;

        [SerializeField]
        private float jumpSpeed;

        private CartoonHoleManager _holeManager;

        private Movement _playerMovement;

        private void Start()
        {
            _holeManager = FindObjectOfType<CartoonHoleManager>();
            _playerMovement = FindObjectOfType<Movement>();
        }

        IEnumerator GetMeaning() // TODO: busy wait is not the correct method here.
        {
            while (!_playerMovement.FellToWorld)
            {
                yield return new WaitForSeconds(0.2f);
            }

            UseOnEnd = true;
            Interact();
        }

        protected override void ScriptInteract()
        {
            if (_playerMovement.FellToWorld)
            {
                return;
            }

            UseOnEnd = false;
            StartCoroutine(_playerMovement.ChangePosition(jumpNewPosition, jumpSpeed));
            StartCoroutine(GameManager._shared.ThrowPlayerOnWorld());
            _holeManager.CloseCircle(duration);
            StartCoroutine(GetMeaning());
        }
    }
}