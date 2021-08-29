using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Moving
{
    [RequireComponent(typeof(MoveParametersContainer))]
    public class MoveController : MonoBehaviour
    {
        private const int CornersArrayLength = 5;
        private const float MinimalMoveDistance = .3f;
        
        public event Action<Vector3> SpeedChanged;
        public event Action<Vector3> MoveTargetChanged;
        
        private Rigidbody _rigidbody;
        private MoveParametersContainer _moveParameters;
        private NavMeshPath _path;
        private Coroutine _moveCor;

        private float StopDistance => Mathf.Pow(_rigidbody.velocity.magnitude, 2) / (2 * _moveParameters.Deceleration) +.1f;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _moveParameters = GetComponent<MoveParametersContainer>();
            _path = new NavMeshPath();
            
            if (!_rigidbody || !_moveParameters)
            {
                Debug.LogError($"No rigidbody on {gameObject.name}");
            }
        }

        private void FixedUpdate()
        {
            SpeedChanged?.Invoke(_rigidbody.velocity);
        }

        /// <summary>
        /// For AI movement
        /// </summary>
        /// <param name="pos"></param>
        public void MoveTo(Vector3 pos)
        {
            if (!NavMesh.CalculatePath(transform.position, pos, NavMesh.AllAreas, _path))
            {
                return;
            }
            
            var corners = new Vector3[CornersArrayLength];
            var cornersCount =  _path.GetCornersNonAlloc(corners);
                
            if (cornersCount >= 1)
            {
                for (int i = 0; i < cornersCount; i++)
                {
                    var distance = Vector3.Distance(corners[i], transform.position);

                    if (!(distance > MinimalMoveDistance))
                    {
                        continue;
                    }
                    
                    MoveTo(corners[i], i == cornersCount - 1);
                    break;
                }
            }
        }
        
        /// <summary>
        /// For player movement
        /// </summary>
        /// <param name="pos"></param>
        public void StartMovingCor(Vector3 pos)
        {
            if (!NavMesh.CalculatePath(transform.position, pos, NavMesh.AllAreas, _path))
            {
                return;
            }
            
            var corners = new Vector3[CornersArrayLength];
            var cornersCount =  _path.GetCornersNonAlloc(corners);

            if (_moveCor != null)
            {
                StopCoroutine(_moveCor);
            }
                
            _moveCor = StartCoroutine(MoveCor(cornersCount, corners, pos));
        }

        public void StopMovingCor()
        {
            if (_moveCor != null)
            {
                StopCoroutine(_moveCor);
                _moveCor = null;
            }
        }
        
        private IEnumerator MoveCor(int cornersCount, Vector3[] cornersArray, Vector3 pos)
        {
            var arrayCompletelyFilled = cornersCount <= cornersArray.Length;
            
            var length = arrayCompletelyFilled ? cornersCount : cornersArray.Length;

            for (int i = 0; i < length; i++)
            {
                var cornerPos = cornersArray[i];
                var lastCorner = arrayCompletelyFilled && i == length - 1;

                float distance = 99;

                while (distance > MinimalMoveDistance)
                {
                    distance = MoveTo(cornerPos, lastCorner);
                    yield return null;
                }
            }

            if (!arrayCompletelyFilled)
            {
                StartMovingCor(pos);
                yield break;
            }
            else
            {
                _moveCor = null;
            }
        }
        
        private float MoveTo(Vector3 cornerPos, bool lastCorner)
        {
            MoveTargetChanged?.Invoke(cornerPos);
            
            var currentPos = transform.position;
            var direction = cornerPos - currentPos;
            var distance = direction.magnitude;

            var currentVelocity = _rigidbody.velocity;
            var currentSpeedToTarget = Vector3.Project(currentVelocity, direction).magnitude;
            var diff = _moveParameters.MaxSpeed - currentSpeedToTarget;

            if (diff < 0)
            {
                return distance;
            }

            float needAcc;

            if (!lastCorner || distance > StopDistance)
            {
                needAcc = _moveParameters.Acceleration;
            }
            else
            {
                needAcc = -Mathf.Pow(currentVelocity.magnitude, 2) / (2 * distance);
            }

            var additiveSpeed = Time.fixedDeltaTime * needAcc;
            if (additiveSpeed > diff)
            {
                additiveSpeed = diff;
            }

            _rigidbody.velocity += direction.normalized * additiveSpeed;
            return distance;
        }
    }
}