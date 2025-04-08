using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MH.UISystem
{

    public enum EVisibleState
    {
        Appearing,
        Appeared,
        Disappearing,
        Disappeared
    }

    public interface IViewModel
    {

    }
    
    
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public abstract class UIView : MonoBehaviour 
    {
        #region ------------ Fields --------------

        private UIViewAnimator _animator;
        private CanvasGroup _canvasGroup;

        public float LastShowTime;
        public EVisibleState EVisibleState { get; private set; } = EVisibleState.Disappeared;

        public UILayer ParentLayer;

        #endregion

        #region ------- Unity Methdos ---------

        private void Awake()
        {
            RegisterEvents();
        }

        private void OnDestroy()
        {
            
        }

        #endregion


        #region ----------- Public Methods --------------

        /// <summary>
        /// This logic is executed when the UI View is activated.
        /// Activate game object -> Change to Appearing State -> Send AppearEvent -> Wait for pre-processing logic -> Proceed with Show animation -> Change to Appeared State -> Send AppearedEvent 
        /// </summary>
        public async UniTask ShowAsync(bool useAnimation = true)
        {
            // We cannot show the view if it is already appeared or appearing.
            if (EVisibleState is EVisibleState.Appearing or EVisibleState.Appeared)
                return;

            LastShowTime = Time.time;

            EVisibleState = EVisibleState.Appearing;
            //_preappear.onnext(this); // publish a event when start appear

            // reset View Status to normal
            var rectTransform = (RectTransform)transform;
            await InitializeRectTransformAsync(rectTransform);

            gameObject.SetActive(true);

            if (useAnimation && _animator != null)
                await _animator.AnimateShowAsync();

            EVisibleState = EVisibleState.Appeared;
            //_postAppear.OnNext(this); // call a event when appear process completed
        }

        /// <summary>
        /// This logic is executed when the UI View is activated.
        /// Change to Disappearing State -> Send DisappearEvent -> Proceed with Hide animation -> Wait for post-processing logic -> Change to Disappeared State -> Send DisappearedEvent
        /// </summary>
        public async UniTask HideAsync(bool useAnimation = true)
        {
            // We cannot hide the view if it is already disappeared or disappearing.
            if (EVisibleState is EVisibleState.Disappeared or EVisibleState.Disappearing)
                return;

            EVisibleState = EVisibleState.Disappearing;
            //_postDisappear.OnNext(this);

            await UniTask.Yield(cancellationToken: this.GetCancellationTokenOnDestroy());

            if (useAnimation && _animator != null)
                await _animator.AnimateHideAsync();

            gameObject.SetActive(false);

            EVisibleState = EVisibleState.Disappeared;
            //_postAppear.OnNext(this);
        }

        public virtual void LoadViewModel(IViewModel viewModel)
        {
            
        }
            
        
        #endregion
        
        /// <summary>
        /// call at Awake()
        /// </summary>
        protected virtual void RegisterEvents()
        {
            _animator = GetComponentInChildren<UIViewAnimator>();
        }

        /// <summary>
        /// Reset RectTransform to default values.
        /// </summary>
        /// <param name="rectTransform"></param>
        private async UniTask InitializeRectTransformAsync(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            await UniTask.Yield();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.localRotation = Quaternion.identity;
        }
    }


    // public abstract class UIView<TViewModel> : UIView where TViewModel : IViewModel
    // {
    //
    //     [SerializeField] private TViewModel _viewModel;
    //
    //     #region ------------ Public Methods -----------
    //
    //     public virtual void SetViewModel(TViewModel viewModel)
    //     {
    //         _viewModel = viewModel;
    //     }
    //
    //     #endregion
    // }
}
