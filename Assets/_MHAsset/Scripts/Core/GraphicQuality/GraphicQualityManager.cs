using UnityEngine;

namespace MH.GraphicQuality
{
    public enum EGraphicQuality
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Ultra = 3
    }
    
    
    public interface IGraphicQualityManager
    {
        /// <summary>
        /// Sets the graphics quality to a specific level.
        /// </summary>
        /// <param name="levelIndex">The index from QualitySettings (0 = lowest)</param>
        void SetGraphicsQuality(int levelIndex);
        
        EGraphicQuality CurrentQuality { get; }
    }
    
    public class GraphicQualityManager : IGraphicQualityManager
    {
        
        private int currentLevelIndex;

        public EGraphicQuality CurrentQuality
        {
            get
            {
                int qualityLevel = QualitySettings.GetQualityLevel();
                return (EGraphicQuality)qualityLevel;
            }
        }

        /// <summary>
        /// Sets the graphics quality to a specific level.
        /// </summary>
        /// <param name="levelIndex">The index from QualitySettings (0 = lowest)</param>
        public void SetGraphicsQuality(int levelIndex)
        {
            if (levelIndex < 0 || levelIndex >= QualitySettings.names.Length)
            {
                Debug.LogWarning("Invalid quality level index: " + levelIndex);
                return;
            }

            QualitySettings.SetQualityLevel(levelIndex, true); // true = apply expensively
            currentLevelIndex = levelIndex;
            Debug.Log("Graphics quality set to: " + QualitySettings.names[levelIndex]);
        }
    }
}