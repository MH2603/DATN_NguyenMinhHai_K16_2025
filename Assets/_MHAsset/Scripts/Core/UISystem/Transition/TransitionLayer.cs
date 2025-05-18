
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace MH.UISystem
{
    public class TransitionLayer : UILayer<TransitionLayer>
    {
        public async UniTask<TView> ShowAsync<TView>(
            IViewModel viewModel = null)
            where TView : UIView
        {
            if (!TryGetContextInstance<TView>(out var transitionInstance))
            {
                Debug.LogError($"Transition not found: {typeof(TView)}");
                return null;
            }

            var previousView = CurrentView;
            //var result = await ShowAsyncInternal(windowInstance, viewModel, onPreInitialize, onPostInitialize);
            if (IsCurrentViewBusy())
            {
                return null;
            }

            // update CurrentView == windowInstance
            //  update Model of context if viewModel != null
            SetupViewInstance(transitionInstance, viewModel);

            await CurrentView.ShowAsync();


            if (previousView != null && previousView.EVisibleState == EVisibleState.Appeared)
            {
                await previousView.HideAsync();
            }

            return transitionInstance;
        }

        /// <summary>
        /// Hides the specified UI context instance asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the UI context instance.</typeparam>
        /// <returns>A UniTask representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the UI context instance can't be found.</exception>
        public async UniTask<T> HideAsync<T>() where T : UIView
        {
            if (_viewRuntimeMap.TryGetValue(typeof(T), out var transitionInstance))
            {
                await transitionInstance.HideAsync();
                CurrentView = null;
                return transitionInstance as T;
            }

            Debug.LogError($"Transition not found: {typeof(T)}");
            return null;
        }
    }
}
