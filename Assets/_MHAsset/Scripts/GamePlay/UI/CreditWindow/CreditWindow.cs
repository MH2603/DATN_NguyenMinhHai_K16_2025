using UnityEngine;
using UnityEngine.UI;

namespace MH.UISystem
{
    public class CreditWindow : UIView
    {
        #region ------------- Fields -------------

        [Header("-------- Credit Window --------")]
        [SerializeField] private Button backBtn;

        #endregion

        #region Private Methods ---------

        protected override void RegisterEvents()
        {
            base.RegisterEvents();

            backBtn.onClick.AddListener(() => WindowLayer.Main.BackAsync());
        }

        #endregion
    }
}