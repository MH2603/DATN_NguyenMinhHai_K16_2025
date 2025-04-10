using UnityEngine;

namespace MH.UISystem
{
    /// <summary>
    /// Abstract base class representing a UI widget element in the UI system.
    /// Intended to be inherited by specific widget types (e.g. button, label, panel).
    /// </summary>
    public abstract class UIWidget : MonoBehaviour
    {
        /// <summary>
        /// Unique identifier for this widget, used for lookup or reference within the UI system.
        /// 
        /// Note:
        /// - The field is serialized to allow assignment via the Inspector.
        /// - Property is publicly readable but privately set to maintain encapsulation.
        /// </summary>
        [field: SerializeField]
        public string ID { get; private set; }
    }
}
