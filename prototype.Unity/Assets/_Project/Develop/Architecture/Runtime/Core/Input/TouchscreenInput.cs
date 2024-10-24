using System;
using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace _Project.Develop.Architecture.Runtime.Core.Input
{
    public class TouchscreenInput : MonoBehaviour
    {
        
        public Camera camera; //    do Inject

        public UnityEvent OnTrainMoveEvent = new();

        private GameInput _input;
        

        private bool _isControlEnable;

        private void Awake()
        {
            _input = new GameInput();
            _input.Core.MoveTrain.performed += context => OnMove();
            Log.Battle.D("Init");
        }
        
        private void OnMove()
        {
            var col = GetTouchedGameObject();

            if (col.gameObject.name == "Trail")
            {
                OnTrainMoveEvent.Invoke();
            }
        }

        private Collider GetTouchedGameObject()
        {
            RaycastHit hit;
            var ray = camera.ScreenPointToRay(Touch.activeTouches[0].screenPosition);
            if (Physics.Raycast(ray, out hit))
            {
                return hit.collider != null ? hit.collider : null;
            }
            else
            {
                return null;
            }
        }

        private void OnEnable()
        {
            _input.Core.Enable();
        }

        private void OnDisable()
        {
            _input.Core.Disable();
        }
    }
}