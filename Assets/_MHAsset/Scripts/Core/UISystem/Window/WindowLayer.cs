using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace MH.UISystem
{

    public class WindowLayer : UILayer<WindowLayer>
    {
        #region -------------------- Fields -------------------

        private readonly Stack<UIView> _windowStack = new();

        #endregion

        /// <summary>
        /// Asynchronously shows a window of type <typeparamref name="TView"/> with the specified view model and optional pre and post-initialization actions.
        /// </summary>
        /// <typeparam name="TView">The type of the window context.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="viewModel">The view model to be passed to the window context.</param>
        /// <param name="onPreInitialize">Optional action to be executed before the window initialization.</param>
        /// <param name="onPostInitialize">Optional action to be executed after the window initialization.</param>
        /// <returns>A <see cref="UniTask{T}"/> representing the asynchronous operation. The result is the window context instance.</returns>
        public async UniTask<TView> ShowAsync<TView>(
            IViewModel viewModel = null)
            where TView : UIView
        {
            if (!TryGetContextInstance<TView>(out var windowInstance))
            {
                Debug.LogError($"Window not found: {typeof(TView)}");
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
            SetupViewInstance(windowInstance, viewModel);

            await CurrentView.ShowAsync();


            if (previousView != null && previousView.EVisibleState == EVisibleState.Appeared)
            {
                await HandlePreviousView(previousView, true);
            }

            return windowInstance;
        }

        /// <summary>
        /// Asynchronously goes back to the previous window in the window stack.
        /// </summary>
        /// <param name="clearStack">If set to <c>true</c>, clear the entire window stack.</param>
        public async UniTask BackAsync(bool clearStack = false)
        {
            if (clearStack)
            {
                _windowStack.Clear();
            }

            if (CurrentView == null)
            {
                Debug.LogError("No window to go back to.");
                return;
            }

            await CurrentView.HideAsync();

            if (_windowStack.TryPop(out var previousView))
            {
                await previousView.ShowAsync();
                CurrentView = previousView;
            }
            else
            {
                CurrentView = null;
            }
        }
        

        private async UniTask HandlePreviousView(UIView previousView, bool stackToWindowLayer)
        {
            // add previous view on top of stack
            if (stackToWindowLayer)
            {
                _windowStack.Push(previousView);
            }

            await previousView.HideAsync();
        }
    }

}
