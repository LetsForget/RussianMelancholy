using System;
using System.Collections;
using UnityEngine;

namespace Characters.Moving
{
    [RequireComponent(typeof(MoveParametersContainer))]
    public class RotateController : MonoBehaviour
    {
        public event Action<float> TurnChanged;

        private MoveParametersContainer _moveParameters;

        private void Start()
        {
            _moveParameters = GetComponent<MoveParametersContainer>();
        }

        public void StartRotateCor(Vector3 pos)
        {
            var currentDirection = transform.forward;
            currentDirection.y = 0;

            var needDirection = pos - transform.position;
            needDirection.y = 0;

            var angleToRotate = Vector3.SignedAngle(currentDirection, needDirection, Vector3.up);

            TurnChanged?.Invoke(angleToRotate > 0 ? .5f : -.5f);
            StartCoroutine(RotateCor(angleToRotate));
        }

        private IEnumerator RotateCor(float angleToRotate)
        {
            Vector3 up = Vector3.up;
            
            if (angleToRotate < 0)
            {
                angleToRotate = -angleToRotate;
                up = -up;
            }
            
            while (angleToRotate != 0)
            {
                var step = _moveParameters.TurnSpeed * Time.fixedDeltaTime;
                
                if (angleToRotate - step < 0)
                {
                    step = angleToRotate;
                }
                
                transform.rotation *= Quaternion.AngleAxis(step, up);
                angleToRotate -= step;
                yield return null;
            }
            
            TurnChanged?.Invoke(0);
        }

        public void RotateTo(Vector3 pos)
        {
            var currentDirection = transform.forward;
            currentDirection.y = 0;

            var needDirection = pos - transform.position;
            needDirection.y = 0;

            var angleToRotate = Vector3.SignedAngle(currentDirection, needDirection, Vector3.up);

            Vector3 up = Vector3.up;

            if (angleToRotate < 0)
            {
                angleToRotate = -angleToRotate;
                up = -up;
            }

            var step = _moveParameters.TurnSpeed * Time.fixedDeltaTime;

            if (angleToRotate - step < 0)
            {
                step = angleToRotate;
            }

            transform.rotation *= Quaternion.AngleAxis(step, up);
        }

        private void OnDestroy()
        {
            TurnChanged = null;
        }
    }
}