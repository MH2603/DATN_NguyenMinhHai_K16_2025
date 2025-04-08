using Cysharp.Threading.Tasks;
using MH.UISystem;
using UnityEngine;

namespace MH.UISystem
{

    public class HUDLayer : UILayer<HUDLayer>
    {
        /// <summary>
        /// Shows the specified HUD instance asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the HUD instance.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="viewModel">The view model instance (optional).</param>
        /// <param name="onPreInitialize">The action to be invoked before initialization (optional).</param>
        /// <param name="onPostInitialize">The action to be invoked after initialization (optional).</param>
        /// <returns>A UniTask representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the HUD instance can't be found.</exception>
        public async UniTask<T> ShowAsync<T>(
            IViewModel viewModel = null)
            where T : UIView
        {
            if (!TryGetContextInstance<T>(out var hudInstance))
            {
                Debug.LogError($"HUD not found: {typeof(T)}");
                return null;
            }

            SetupViewInstance(hudInstance, viewModel);

            await CurrentView.ShowAsync();

            return hudInstance;
        }

        /// <summary>
        /// Hides the specified UI context instance asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the UI context instance.</typeparam>
        /// <returns>A UniTask representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the UI context instance can't be found.</exception>
        public async UniTask<T> HideAsync<T>() where T : UIView
        {
            if (_viewRuntimeMap.TryGetValue(typeof(T), out var hudInstance))
            {
                await hudInstance.HideAsync();
                return hudInstance as T;
            }

            Debug.LogError($"HUD not found: {typeof(T)}");
            return null;
        }
    }

}
