using DG.Tweening;
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
        private RectTransform rectTransform;

        private void Start()
        {
            rectTransform = (RectTransform)transform;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            rectTransform.DOLocalJump(rectTransform.localPosition, 10f, 1, 0.1f);
            onClick?.Invoke();
        }
    }
}
