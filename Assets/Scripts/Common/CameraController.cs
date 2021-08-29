using UnityEngine;

namespace Common
{
    public class CameraController : MonoBehaviour
    {
        private static CameraController _instance;

        [SerializeField] private GameObject mainHero;

        [SerializeField] private float tiltAngle;
        [SerializeField] private float rotateAngle;
        [SerializeField] private float height;
        [SerializeField] private float lerpSpeed;
        
        public static Camera Camera => _instance._camera;
        private Camera _camera;
        
        private void Start()
        {
            if (_instance)
            {
                Debug.LogWarning("More than one camera controller!");
                Destroy(this);
                return;
            }

            _instance = this;
            _camera = GetComponent<Camera>();

            transform.rotation = Quaternion.Euler(tiltAngle,rotateAngle, 0);
        }

        private void Update()
        {
            var mhPos = mainHero.transform.position;
            
            var camForward = transform.forward;
            var camPosition = transform.position;
            
            var multiplier = (mhPos.y - height) / camForward.y;
            var camNeedPos = mhPos - camForward * multiplier;;
            transform.position = Vector3.Lerp(camPosition, camNeedPos, lerpSpeed * Time.deltaTime);
        }
    }
}
