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
        
        [Header("Visual Elements")]
        [SerializeField] private RectTransform joystickBackground;
        [SerializeField] private RectTransform joystickHandle;
        
        private Vector2 inputVector;
        private Vector2 joystickCenter;
        private bool isActive = false;
        
        private void Start()
        {
            if (joystickBackground == null)
                joystickBackground = GetComponent<RectTransform>();
                
            if (joystickHandle == null && transform.childCount > 0)
                joystickHandle = transform.GetChild(0).GetComponent<RectTransform>();
                
            // Get the center position
            joystickCenter = joystickBackground.position;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            isActive = true;
            OnDrag(eventData);
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