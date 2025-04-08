using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MH.UISystem
{

    [RequireComponent(typeof(CanvasGroup))]
    public class UIViewAnimator : MonoBehaviour
    {
        #region Fields

        [SerializeReference] public BaseViewAnimation[] _showAnimations;
        [SerializeReference] public BaseViewAnimation[] _hideAnimations;
        

        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        
        #endregion


        #region ------ Unity Methods ------

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        #endregion

        #region Public Methods

        public async UniTask AnimateShowAsync()
        {
            await AnimateAsync(_showAnimations);
        }

        public async UniTask AnimateHideAsync()
        {
            await AnimateAsync(_hideAnimations);
        }

        // /// <summary>
        // /// Animates the given Rect transform and canvas group using the move animations specified in the <see cref="UIViewAnimator"/> scriptable object.
        // /// </summary>
        // /// <param name="rectTransform">The Rect transform to animate.</param>
        // /// <param name="canvasGroup">The canvas group to animate.</param>
        // /// <returns>A <see cref="UniTask"/> representing the asynchronous operation.</returns>
        // public async UniTask AnimateAsync(RectTransform rectTransform, CanvasGroup canvasGroup)
        // {
        //     var tasks = new UniTask[_viewAnimations.Length];
        //     for (var i = 0; i < _viewAnimations.Length; i++)
        //     {
        //         tasks[i] = _viewAnimations[i].AnimateAsync(rectTransform, canvasGroup);
        //     }
        //     await UniTask.WhenAll(tasks);
        // }
        
        private async UniTask AnimateAsync(BaseViewAnimation[] animations)
        {
            var tasks = new UniTask[animations.Length];
            for (var i = 0; i < animations.Length; i++)
            {
                tasks[i] = animations[i].AnimateAsync(_rectTransform, _canvasGroup);
            }
            await UniTask.WhenAll(tasks);
        }

        #endregion
    }
}
