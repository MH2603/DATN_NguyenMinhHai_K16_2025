using UnityEngine;

namespace MH
{
    public class AutoRotate : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotationSpeed = new Vector3(0, 50, 0);
        
        private void Update()
        {
            // Rotate the object around its local Y axis at the specified speed
            transform.Rotate(_rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}