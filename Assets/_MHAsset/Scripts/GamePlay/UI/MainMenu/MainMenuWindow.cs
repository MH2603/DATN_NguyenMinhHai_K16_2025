using MH.ChapterSystem;
using MH.SaveSystem;
using System;
using UnityEngine;

namespace MH.UISystem
{

    public class MainMenuViewModel : IViewModel
    {
        public bool IsShowContinueBtn;
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

        private ISaveSystem _saveSystem => ServiceLocator.Get<ISaveSystem>();
        private IChapterManager _chapterManager => ServiceLocator.Get<IChapterManager>();

        #endregion


        #region ------------- Public Methods --------------
        public override void LoadViewModel(IViewModel viewModel)
        {
            base.LoadViewModel(viewModel);

            var vModel = viewModel as MainMenuViewModel;

            _continueBtn.gameObject.SetActive(vModel.IsShowContinueBtn);
        }

        #endregion



        #region ---------- Private Methods ---------

        protected override void RegisterEvents()
        {
            base.RegisterEvents();

            _newGameBtn.onClick.AddListener(OnNewGameBtnPressed);
            _continueBtn.onClick.AddListener(OnContinueBtnPressed);
        }

        private void OnContinueBtnPressed()
        {
            var save = _saveSystem.GameSave;  
            _chapterManager.SwithChapter(save.Chapter, save.CheckPoint);

            WindowLayer.Main.BackAsync();
        }

        private void OnNewGameBtnPressed()
        {
            _saveSystem.Clean();
            _chapterManager.SwithChapter(0);

            WindowLayer.Main.BackAsync();
        }

        #endregion
    }
}