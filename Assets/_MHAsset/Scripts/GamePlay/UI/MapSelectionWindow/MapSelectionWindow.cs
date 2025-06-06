using UnityEngine;
using UnityEngine.UI;

namespace MH.UISystem
{
    public class MapSelectionWindow : UIView
    {
        [SerializeField] private UIMapSelecter[] mapSelecters;
        [SerializeField] private Button backBtn;
        
        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            
            backBtn.onClick.AddListener(() => WindowLayer.Main.BackAsync());
        }
    }
}