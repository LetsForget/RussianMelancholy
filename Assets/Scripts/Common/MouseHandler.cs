using System;
using UnityEngine;

namespace Common
{
    public class MouseHandler : MonoBehaviour
    {
        public static event Action<RaycastHit> GroundClick;

        private static MouseHandler _instance;
        
        private void Start()
        {
            if (_instance)
            {
                Debug.LogWarning("More than one mouse controller!");
                Destroy(this);
                return;
            }

            _instance = this;
            
            InputHandler.LMBPressed += OnLMBPressed;
        }

        private void OnLMBPressed()
        {
            var screenPos = Input.mousePosition;
            var ray = CameraController.Camera.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out var hit))
            {
                GroundClick?.Invoke(hit);
            }
        }

        private void OnDestroy()
        {
            GroundClick = null;
        }
    }
}