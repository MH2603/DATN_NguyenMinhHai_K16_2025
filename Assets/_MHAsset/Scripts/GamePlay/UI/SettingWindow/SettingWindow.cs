using MH.GraphicQuality;
using MH.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace MH.UISystem
{
    
    
    public class SettingWindow : UIView
    {
        #region ---------------- Fields -------------

        [Header("------------ Setting Window ------------")] 
        [SerializeField] private Slider soundSlider;
        [SerializeField] private Slider graphicsSlider;
        [SerializeField] private Button backButton;

        
        private IGraphicQualityManager graphicQualityManager => ServiceLocator.Get<IGraphicQualityManager>();
        private ISoundManager soundManager => ServiceLocator.Get<ISoundManager>();
        #endregion


        #region ---------- Private Methods -------------

        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            
            soundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);
            graphicsSlider.onValueChanged.AddListener(OnGraphicsSliderValueChanged);
            backButton.onClick.AddListener(()=> WindowLayer.Main.BackAsync());
        }

        
        private void OnSoundSliderValueChanged(float value)
        {
            // Handle sound slider value change
            Debug.Log($"Sound slider value changed: {value}");
            
            soundManager.SetSoundVolume(value);
        }
        
        private void OnGraphicsSliderValueChanged(float value)
        {
            // Handle graphics slider value change
            graphicQualityManager.SetGraphicsQuality((int)value);
        }
        #endregion
    }
}