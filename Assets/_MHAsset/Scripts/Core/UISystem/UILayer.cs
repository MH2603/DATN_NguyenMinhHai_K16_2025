using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace MH.UISystem
{

    /// <summary>
    /// The base class for all UI layers in the application.
    /// We're using CRTP to allow for easy access to the main instance of the layer.
    /// </summary>
    public abstract class UILayer : MonoBehaviour
    {
        #region ------------- Fields -------------

        [SerializeField] private UIView[] _viewPrefabs;

        protected readonly Dictionary<Type, UIView> _viewRuntimeMap = new();
        protected readonly Dictionary<Type, UIView> _viewPrefabMap = new();

        public UIView CurrentView { get; protected set; }

        #endregion


        #region ------------ Unity Methods ------------



        #endregion

        #region ----------- Public Methods --------------

        public virtual void Initialized()
        {
            InitializeViewInstances();
        }

        #endregion

        #region ------------- protected Methods ----------------

        protected void InitializeViewInstances()
        {
            
            foreach (var viewPrefab in _viewPrefabs)
            {
                _viewPrefabMap.Add(viewPrefab.GetType(), viewPrefab);
                GetOrCreateContextInstance(viewPrefab);
            }
        }

        protected bool IsCurrentViewBusy()
        {
            return CurrentView != null &&
                   (CurrentView.EVisibleState == EVisibleState.Appearing ||
                    CurrentView.EVisibleState == EVisibleState.Disappearing);
        }

        /// <summary>
        /// Tries to get an instance of the specified UI context type.
        /// </summary>
        /// <typeparam name="TView">The type of the UI context.</typeparam>
        /// <param name="contextInstance">The instance of the UI context.</param>
        /// <returns><c>true</c> if the instance is found or created successfully; otherwise, <c>false</c>.</returns>
        protected bool TryGetContextInstance<TView>(out TView contextInstance) where TView : UIView
        {
            contextInstance = null;
            if (!_viewPrefabMap.TryGetValue(typeof(TView), out var context))
            {
                return false;
            }

            if (!_viewRuntimeMap.TryGetValue(typeof(TView), out var instance))
            {
                instance = GetOrCreateContextInstance(context);
            }

            contextInstance = instance as TView;
            return true;
        }

        protected TView GetOrCreateContextInstance<TView>(TView context) where TView : UIView
        {
            if (!_viewRuntimeMap.TryGetValue(typeof(TView), out var contextInstance))
            {
                context.gameObject.SetActive(false);
                contextInstance = Instantiate(context, transform);
                context.gameObject.SetActive(true);
                _viewRuntimeMap.Add(context.GetType(), contextInstance);
            }

            return contextInstance as TView;
        }

        protected void SetupViewInstance<TView>(
            TView viewInstance,
            IViewModel viewModel
            )
            where TView : UIView
        {
            //viewInstance.OnPreAppear.Subscribe(_ => onPreInitialize?.Invoke(viewInstance)).AddTo(viewInstance);

            if (viewModel != null)
            {
                viewInstance.LoadViewModel(viewModel);
            }

            //viewInstance.OnPostAppear.Subscribe(_ => onPostInitialize?.Invoke(viewInstance)).AddTo(viewInstance); 

            viewInstance.ParentLayer = this;
            CurrentView = viewInstance;
        }

        #endregion
    }

    public class UILayer<T> : UILayer where T : UILayer<T>
    {
        /// <summary>
        /// Singleton instance of the UI Layer.
        /// </summary>
        public static T Main
        {
            get
            {
                T main = UIManager.Instance.GetLayer<T>();
                if (main) return main;
                return null;
            }
        }


    }

}
