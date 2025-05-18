using MH.ChapterSystem;
using MH.SaveSystem;
using System;
using MH.GameState;
using UnityEngine;

namespace MH.UISystem
{

    public class MainMenuViewModel : IViewModel
    {
        public bool IsShowContinueBtn;
        public Action OnNewGameBtnClicked;
        public Action OnContinueBtnClicked;
        public Action OnOptionBtnClicked;
        public Action OnCreditBtnClicked;
        public Action OnExitGameBtnClicked;
    }
    
    
    public class MainMenuWindow : UIView
    {
        #region ----------- Fields -----------
        [Header("--------- Buttons ------------")]
        [SerializeField] private CustomButton _continueBtn;
        [SerializeField] private CustomButton _newGameBtn;
        [SerializeField] private CustomButton _optionBtn;
        [SerializeField] private CustomButton _creditBtn;
        [SerializeField] private CustomButton _quietBtn;

        

        #endregion


        #region ------------- Public Methods --------------
        public override void LoadViewModel(IViewModel viewModel)
        {
            base.LoadViewModel(viewModel);
            Debug.Log("Main Menu Window: Load View Model");
            var vModel = viewModel as MainMenuViewModel;

            _continueBtn.gameObject.SetActive(vModel.IsShowContinueBtn);
            
            _continueBtn.onClick.RemoveAllListeners();
            _continueBtn.onClick.AddListener(() => vModel.OnContinueBtnClicked());
            
            _newGameBtn.onClick.RemoveAllListeners();
            _newGameBtn.onClick.AddListener(() => vModel.OnNewGameBtnClicked());
            
            _optionBtn.onClick.RemoveAllListeners();
            _optionBtn.onClick.AddListener(() => vModel.OnOptionBtnClicked());
            
            _creditBtn.onClick.RemoveAllListeners();
            _creditBtn.onClick.AddListener(() => vModel.OnCreditBtnClicked());
            
            _quietBtn.onClick.RemoveAllListeners();
            _quietBtn.onClick.AddListener(() => vModel.OnExitGameBtnClicked());
        }

        #endregion



        #region ---------- Private Methods ---------

      
        #endregion
    }
}