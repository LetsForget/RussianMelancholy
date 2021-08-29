using UnityEngine;

namespace Common
{
    public class Pool<T> where T : MonoBehaviour
    {
        private readonly T _prefab;
        private readonly T[] _array;
        private int _currentIndex = 0;
        
        public Pool(int count, T prefab)
        {
            _array = new T[count];
            this._prefab = prefab;
        }

        public T Get(Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            if (!_array[_currentIndex])
            {
                _array[_currentIndex] = Object.Instantiate(_prefab, position, rotation, parent);
            }
            else
            {
                _array[_currentIndex].transform.position = position;
                _array[_currentIndex].transform.rotation = rotation;
                _array[_currentIndex].transform.parent = parent;
            }

            var bottle = _array[_currentIndex];
            
            _currentIndex++;
            if (_currentIndex >= _array.Length)
            {
                _currentIndex = 0;
            }

            return bottle;
        }
    }
}