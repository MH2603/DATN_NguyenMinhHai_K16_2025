using UnityEngine;

namespace MH.Portal
{
    public class FPSConfig : MonoBehaviour
    {
        [SerializeField] private int targetFrameRate = 60;
        [SerializeField] private bool enableVSync = false;

        void Start()
        {
            ApplyFPSSettings();
        }

        void ApplyFPSSettings()
        {
            if (enableVSync)
            {
                QualitySettings.vSyncCount = 1; // Enable VSync
                Application.targetFrameRate = -1; // Ignore target frame rate when VSync is enabled
            }
            else
            {
                QualitySettings.vSyncCount = 0; // Disable VSync
                Application.targetFrameRate = targetFrameRate; // Set target frame rate
            }
        }
    }
}