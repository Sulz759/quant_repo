using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Develop.Architecture.Runtime.Core.Input
{
    public class TachscreenInput : MonoBehaviour, IInput, IPointerClickHandler
    {
        private GameInput _input;

        private void Awake()
        {
            _input = new GameInput();
            _input.Core.MoveTrain.performed += context => MoveTrain();
        }

        private void MoveTrain()
        {
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}