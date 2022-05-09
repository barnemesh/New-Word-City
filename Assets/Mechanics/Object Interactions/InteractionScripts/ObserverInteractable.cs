﻿using System;
using System.Collections.Generic;
using Mechanics.WordBase;
using UnityEngine;

namespace Mechanics.Object_Interactions.InteractionScripts
{
    class ObserverInteractable : EventInteractable
    {
        [Space]
        [Header("Observer")]
        [SerializeField]
        private EventFromScriptTest.RegisterToEvents events;

        [Space]
        [SerializeField]
        private bool unregisterWhenTargetHit = true;

        [SerializeField]
        private int target = 1;

        private int _received;

        public int Target
        {
            get => target;
            set => target = value;
        }

        public int Received => _received;

        protected override void Awake()
        {
            base.Awake();
            events.Register(CallbackEvent);
        }

        private void OnDestroy()
        {
            events.UnRegister(CallbackEvent);
        }

        private void CallbackEvent(object sender, InteractableObject interactableObject)
        {
            _received++;
            Debug.Log($"<color=red>Callback received</color> {Received}", this);
            if (_received < target)
            {
                return;
            }

            CanInteract = true;
            if (unregisterWhenTargetHit)
            {
                events.UnRegister(CallbackEvent);
            }
        }

        protected override void ScriptInteract()
        {
            base.ScriptInteract();
            Debug.Log(_received, this);
        }
    }
}