using UnityEngine;
using UnityEngine.EventSystems;

namespace BrickDigger
{
    public class DynamicJoystickArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [Header("References")]
        [SerializeField] private MobileJoystick joystick;
        
        [Header("Settings")]
        [SerializeField] private bool leftHalfOnly = true; // Only left half of screen
        
        private bool isActive = false;
        
        private void Start()
        {
            if (joystick == null)
            {
                joystick = FindObjectOfType<MobileJoystick>();
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log($"Touch area received touch at: {eventData.position}");
            
            // Check if touch is in valid area
            if (leftHalfOnly && eventData.position.x > Screen.width / 2)
            {
                Debug.Log("Touch ignored - right half");
                return; // Ignore touches on right half
            }
            
            isActive = true;
            Debug.Log("Forwarding to joystick");
            
            // Forward to joystick
            if (joystick != null)
            {
                joystick.OnPointerDown(eventData);
            }
            else
            {
                Debug.LogError("Joystick reference is null!");
            }
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!isActive) return;
            
            isActive = false;
            
            // Forward to joystick
            if (joystick != null)
            {
                joystick.OnPointerUp(eventData);
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (!isActive) return;
            
            // Forward to joystick
            if (joystick != null)
            {
                joystick.OnDrag(eventData);
            }
        }
    }
}