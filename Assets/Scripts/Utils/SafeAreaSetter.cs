using UnityEngine;

namespace Utils
{
    public class SafeAreaSetter : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        private RectTransform panelSafeArea;

        private Rect currentSafeArea;
        private ScreenOrientation currentOrientation = ScreenOrientation.AutoRotation;
    
        private void Start()
        {
            panelSafeArea = GetComponent<RectTransform>();
        
            currentOrientation = Screen.orientation;
            currentSafeArea = Screen.safeArea;
        
            UpdateSafeArea();
        }

        private void UpdateSafeArea()
        {
            if (panelSafeArea == null) return;
        
            var safeArea = Screen.safeArea;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= canvas.pixelRect.width;
            anchorMin.y /= canvas.pixelRect.height;
        
            anchorMax.x /= canvas.pixelRect.width;
            anchorMax.y /= canvas.pixelRect.height;

            panelSafeArea.anchorMin = anchorMin;
            panelSafeArea.anchorMax = anchorMax;

            currentOrientation = Screen.orientation;
            currentSafeArea = Screen.safeArea;
        }

        private void FixedUpdate()
        {
            if ((currentOrientation != Screen.orientation) || (currentSafeArea != Screen.safeArea))
            {
                UpdateSafeArea();
            }
        }
    }
}
