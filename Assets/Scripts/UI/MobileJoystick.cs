using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BrickDigger
{
    public class MobileJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [Header("Joystick Settings")]
        [SerializeField] private float joystickRadius = 100f;
        [SerializeField] private float deadZone = 0.1f;
        [SerializeField] private bool dynamicJoystick = true;
        
        [Header("Visual Elements")]
        [SerializeField] private RectTransform joystickBackground;
        [SerializeField] private RectTransform joystickHandle;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private Vector2 inputVector;
        private Vector2 touchStartPosition;
        private bool isActive = false;
        private Canvas parentCanvas;
        
        private void Start()
        {
            if (joystickBackground == null)
                joystickBackground = GetComponent<RectTransform>();
                
            if (joystickHandle == null && transform.childCount > 0)
                joystickHandle = transform.GetChild(0).GetComponent<RectTransform>();
            
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
            
            parentCanvas = GetComponentInParent<Canvas>();
            
            // Hide joystick initially if dynamic
            if (dynamicJoystick && canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            isActive = true;
            touchStartPosition = eventData.position;
            
            Debug.Log($"Touch at screen position: {eventData.position}");
            
            // Show joystick at touch position
            if (dynamicJoystick)
            {
                // Get the parent canvas
                Canvas canvas = GetComponentInParent<Canvas>();
                RectTransform parentRect = joystickBackground.parent as RectTransform;
                
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentRect,
                    eventData.position,
                    canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : eventData.pressEventCamera,
                    out localPoint
                );
                
                Debug.Log($"Local point: {localPoint}");
                
                joystickBackground.anchoredPosition = localPoint;
                
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1f;
                    Debug.Log("Joystick shown");
                }
            }
            
            // Reset handle to center (don't move character yet)
            if (joystickHandle != null)
            {
                joystickHandle.anchoredPosition = Vector2.zero;
            }
            
            inputVector = Vector2.zero;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            isActive = false;
            inputVector = Vector2.zero;
            
            // Reset handle position
            if (joystickHandle != null)
            {
                joystickHandle.anchoredPosition = Vector2.zero;
            }
            
            // Hide joystick
            if (dynamicJoystick && canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (!isActive) return;
            
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickBackground,
                eventData.position,
                eventData.pressEventCamera,
                out position
            );
            
            // Clamp to joystick radius
            position = Vector2.ClampMagnitude(position, joystickRadius);
            
            // Update handle position
            if (joystickHandle != null)
            {
                joystickHandle.anchoredPosition = position;
            }
            
            // Calculate input vector
            inputVector = position / joystickRadius;
            
            // Apply dead zone
            if (inputVector.magnitude < deadZone)
            {
                inputVector = Vector2.zero;
            }
            else
            {
                // Normalize considering dead zone
                inputVector = inputVector.normalized * ((inputVector.magnitude - deadZone) / (1 - deadZone));
            }
        }
        
        public Vector2 GetInput()
        {
            return inputVector;
        }
        
        public float GetHorizontal()
        {
            return inputVector.x;
        }
        
        public float GetVertical()
        {
            return inputVector.y;
        }
        
        public bool IsActive()
        {
            return isActive;
        }
    }
}