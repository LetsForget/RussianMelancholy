using System;
using UnityEngine;

namespace Common
{
    public class InputHandler : MonoBehaviour
    {
        public static event Action LMBPressed;

        public static bool ShiftPressed => Input.GetKey(KeyCode.LeftShift);
        
        private static InputHandler _instance;
        
        private void Start()
        {
            if (_instance)
            {
                Debug.LogWarning("More than one input controllers!");
                Destroy(this);
                return;
            }

            _instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                LMBPressed?.Invoke();
            }
        }

        private void OnDestroy()
        {
            LMBPressed = null;
        }
    }
}