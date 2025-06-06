using MH.GraphicQuality;
using MH.SaveSystem;
using MH.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MH.UISystem
{
    [System.Serializable]
    public class GraphicQualityBinding
    {
        public string QualityName;
        public Color TextColor;
    }
    
    public class SettingWindow : UIView
    {
        #region ---------------- Fields -------------

        [Header("------------ Setting Window ------------")] 
        [SerializeField] private Slider soundSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider graphicsSlider;
        [SerializeField] private Button backButton;
        
        [Space]
        [SerializeField] private TextMeshProUGUI graphicsQualityText;
        [SerializeField] private GraphicQualityBinding[] bindings;

        
        private IGraphicQualityManager graphicQualityManager => ServiceLocator.Get<IGraphicQualityManager>();
        private ISoundManager soundManager => ServiceLocator.Get<ISoundManager>();
        
        private ISaveSystem saveSystem => ServiceLocator.Get<ISaveSystem>();
        #endregion


        #region ---------- Private Methods -------------

        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            
            soundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);
            musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
            graphicsSlider.onValueChanged.AddListener(OnGraphicsSliderValueChanged);
            backButton.onClick.AddListener(()=> WindowLayer.Main.BackAsync());

            soundSlider.value = saveSystem.GameSave.SoundVolume;
            musicSlider.value = saveSystem.GameSave.MusicVolume;
            graphicsSlider.value = saveSystem.GameSave.GraphicQuality;
        }

        
        private void OnSoundSliderValueChanged(float value)
        {
            // Handle sound slider value change
            //Debug.Log($"Sound slider value changed: {value}");
            
            soundManager.SetSoundVolume(value);
            
            
            // Save the sound volume to the save system
            saveSystem.GameSave.SoundVolume = value;
            saveSystem.Save();
        }
        
        private void OnMusicSliderValueChanged(float value)
        {
            // Handle music slider value change
            //Debug.Log($"Music slider value changed: {value}");
            
            soundManager.SetMusicVolume(value);
            
            // Save the music volume to the save system
            saveSystem.GameSave.MusicVolume = value;
            saveSystem.Save();
        }
        
        private void OnGraphicsSliderValueChanged(float value)
        {
            // Handle graphics slider value change
            graphicQualityManager.SetGraphicsQuality((int)value);
            
            // Save the graphics quality to the save system
            saveSystem.GameSave.GraphicQuality = (int)value;
            saveSystem.Save();
            
            // Update the graphics quality text based on the selected quality
            if (value < 0 || value >= bindings.Length)
            {
                graphicsQualityText.text = "Unknown Quality";
                graphicsQualityText.color = Color.white;
                return;
            }
            
            var binding = bindings[(int)value];
            graphicsQualityText.text = binding.QualityName;
            graphicsQualityText.color = binding.TextColor;
        }
        #endregion
    }
}