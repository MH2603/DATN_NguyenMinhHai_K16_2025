using UnityEngine;

namespace MH
{
    public class AutoSinWave : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private float amplitude = 1f;

        private Vector3 startPosition;

        void Start()
        {
            startPosition = transform.position;
        }

        void FixedUpdate()
        {
            float xOffset = Mathf.Sin(Time.time * speed) * amplitude;
            transform.position = startPosition + Vector3.right * xOffset;
            
        }
    }
}