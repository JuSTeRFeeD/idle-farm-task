using UnityEngine;
using UnityEngine.EventSystems;

namespace Input
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] protected RectTransform background;
        [SerializeField] private RectTransform handle;
        [Space]
        [SerializeField, Min(0)] private float handleRange = 1;
        [SerializeField, Min(0)] private float deadZone = 0;
        
        private Camera mainCam = null;
        private RectTransform rect;
        public Vector2 Direction { get; private set; } = Vector2.zero;
        
        private void Start()
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera) mainCam = canvas.worldCamera;
            rect = GetComponent<RectTransform>();
            background.gameObject.SetActive(false);
            
            var center = new Vector2(0.5f, 0.5f);
            background.pivot = center;
            handle.anchorMin = center;
            handle.anchorMax = center;
            handle.pivot = center;
            handle.anchoredPosition = Vector2.zero;

            var canvasRect = canvas.GetComponent<RectTransform>().rect;
            rect.sizeDelta = new Vector2(canvasRect.width, canvasRect.height);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var position = RectTransformUtility.WorldToScreenPoint(mainCam, background.position);
            var radius = background.sizeDelta / 2;
            Direction = (eventData.position - position) / (radius * canvas.scaleFactor);
            
            var magnitude = Direction.magnitude;
            if (magnitude > deadZone)
            {
                if (magnitude > 1)
                    Direction = Direction.normalized;
            }
            else
            {
                Direction = Vector2.zero;
            }
            
            handle.anchoredPosition = Direction * radius * handleRange;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Direction = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            background.gameObject.SetActive(false);
        }
        
        private Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPosition, mainCam, out var localPoint))
            {
                var pivotOffset = rect.pivot * rect.sizeDelta;
                return localPoint - background.anchorMax * rect.sizeDelta + pivotOffset;
            }
            return Vector2.zero;
        }
    }
}