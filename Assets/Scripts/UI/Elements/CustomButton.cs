using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Elements
{
    [RequireComponent(typeof(Image))]
    public class CustomButton : MonoBehaviour, IPointerDownHandler
    {
        public UnityEvent onClick;

        public void OnPointerDown(PointerEventData eventData)
        {
            onClick?.Invoke();
        }
    }
}
