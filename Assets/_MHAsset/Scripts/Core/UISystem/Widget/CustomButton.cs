using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MH.UISystem
{
    /// <summary>
    /// A custom button that detects common UI interactions like hover, press, release, and click.
    /// </summary>
    [RequireComponent(/*typeof(Collider),*/ typeof(CanvasRenderer))]
    public class CustomButton : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerClickHandler
    {
        [Header("Custom Button Events")]
        public UnityEvent onPointerEnter;    // Called when pointer enters the button
        public UnityEvent onPointerExit;     // Called when pointer exits the button
        public UnityEvent onPointerDown;     // Called when pointer is pressed down on the button
        public UnityEvent onPointerUp;       // Called when pointer is released from the button
        public UnityEvent onClick;           // Called when pointer clicks the button

        /// <summary>
        /// Called when the pointer enters the button area.
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            onPointerEnter?.Invoke();
            //Debug.Log("Pointer Enter");
        }

        /// <summary>
        /// Called when the pointer exits the button area.
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            onPointerExit?.Invoke();
            //Debug.Log("Pointer Exit");
        }

        /// <summary>
        /// Called when the pointer is pressed down on the button.
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            onPointerDown?.Invoke();
            //Debug.Log("Pointer Down");
        }

        /// <summary>
        /// Called when the pointer is released on the button.
        /// </summary>
        public void OnPointerUp(PointerEventData eventData)
        {
            onPointerUp?.Invoke();
            //Debug.Log("Pointer Up");
        }

        /// <summary>
        /// Called when the pointer clicks the button.
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
            //Debug.Log("Pointer Clicked");
        }
    }

}