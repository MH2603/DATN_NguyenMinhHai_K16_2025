using System;
using UnityEngine;

namespace MH.UISystem
{
    public class PauseViewModel : IViewModel
    {
        public Action OnResumeGame;
        public Action OnMainMenu;
        public Action OnOptions;
    }
    
    public class PauseWindow : UIView
    {
        #region  ----------- Fields --------------

        [SerializeField] private CustomButton resumeBtn;
        [SerializeField] private CustomButton mainMenuBtn;
        [SerializeField] private CustomButton optionsBtn;
        

        #endregion
        
        
        #region -------------- Public Methods ----------------
        
        
        public override void LoadViewModel(IViewModel viewModel)
        {
            base.LoadViewModel(viewModel);
            
            var vModel = viewModel as PauseViewModel;
            
            resumeBtn.onClick.RemoveAllListeners();
            mainMenuBtn.onClick.RemoveAllListeners();   
            optionsBtn.onClick.RemoveAllListeners();
            
            resumeBtn.onClick.AddListener(() => vModel.OnResumeGame());
            mainMenuBtn.onClick.AddListener(() => vModel.OnMainMenu());
            optionsBtn.onClick.AddListener(() => vModel.OnOptions());
        }
        
        #endregion
    }
}