using _Project.Develop.Architecture.Runtime.Core.Railway;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using UnityEngine;
using UnityEngine.Events;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace _Project.Develop.Architecture.Runtime.Core.Input
{
    public class TouchscreenInput : MonoBehaviour
    {
        public Camera camera; //    do Inject

        public UnityEvent<WayView> OnTrainMoveEvent = new();

        private GameInput _input;
        

        private bool _isControlEnable;

        private void Awake()
        {
            _input = new GameInput();
            _input.Core.MoveTrain.performed += context => GetTouchedGameObject();
        }
        
        private void GetTouchedGameObject()
        {
            var col = TouchedGameObject().GetComponent<MonoBehaviour>();
            if (col is WayView)
            {
                WayView way = col.GetComponent<WayView>();
                OnTrainMoveEvent.Invoke(way);
            }
            
        }

        private Collider TouchedGameObject()
        {
            RaycastHit hit;
            var ray = camera.ScreenPointToRay(Touch.activeTouches[0].screenPosition);
            if (Physics.Raycast(ray, out hit))
            {
                return hit.collider != null ? hit.collider : null;
            }
            return null;
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