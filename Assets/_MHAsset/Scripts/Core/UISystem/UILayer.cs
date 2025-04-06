
using UnityEngine;

namespace MH.Core.UISystem
{
    /// <summary>
    /// The base class for all UI layers in the application.
    /// </summary>
    public abstract class UILayer : MonoBehaviour
    {
        /// <summary>
        /// Sets the active state of the UI layer.
        /// </summary>
        /// <param name="active">The active state to set.</param>
        /// <returns>A <see cref="UniTask"/> representing the asynchronous operation.</returns>
        public virtual UniTask SetActiveAsync(bool active)
        {
            gameObject.SetActive(active);
            return OnSetActive(active);
        }

        /// <summary>
        /// Override this method to perform additional operations when the active state of the UI layer is set.
        /// </summary>
        /// <param name="active">The active state to set.</param>
        protected virtual UniTask OnSetActive(bool active)
        {
            return UniTask.CompletedTask;
        }
    }


    /// <summary>
    /// The base class for all UI layers in the application.
    /// We're using CRTP to allow for easy access to the main instance of the layer.
    /// </summary>
    public abstract class UILayer<T> : UILayer where T : UILayer<T>
    {
        public static T Main
        {
            get
            {
                T main = UIManager.Instance.GetMain<T>();
                if (main) return main;
                return null;
            }
        }
    }
}
