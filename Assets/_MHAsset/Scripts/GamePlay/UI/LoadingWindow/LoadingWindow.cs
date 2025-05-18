using System;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace MH.UISystem
{
    public class LoadingWindow : UIView
    {
        #region ------------ Fields -------------

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Image bgImage;
        [Space]
        [SerializeField] private Transform loadingIcon;
        [SerializeField] private Vector3 rotationSpeed = new Vector3(0f, 0f, 100f);
        
        #endregion


        #region  ------------- Unity Methods --------------

        private void Update()
        {
            loadingIcon.Rotate(rotationSpeed, Space.Self);
            
            canvasGroup.alpha += 1f/ 2f * Time.deltaTime;
        }

        #endregion
    }
}