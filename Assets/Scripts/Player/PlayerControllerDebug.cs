using UnityEngine;

namespace BrickDigger
{
    public class PlayerControllerDebug : MonoBehaviour
    {
        private void Update()
        {
            // Raycast down to check what we're standing on
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
            {
                string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                Debug.Log($"Standing on: {hit.collider.gameObject.name} (Layer: {layerName})");
            }
            
            // Raycast forward to check what's ahead
            Vector3 forward = transform.forward;
            Vector3 checkPos = transform.position + forward * 0.6f;
            if (Physics.Raycast(checkPos, Vector3.down, out hit, 1f))
            {
                string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                Debug.Log($"Ahead: {hit.collider.gameObject.name} (Layer: {layerName})");
            }
        }
        
        private void OnDrawGizmos()
        {
            // Draw raycast down
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1f);
            
            // Draw raycast forward
            Gizmos.color = Color.blue;
            Vector3 forward = transform.forward;
            Vector3 checkPos = transform.position + forward * 0.6f;
            Gizmos.DrawLine(checkPos, checkPos + Vector3.down * 1f);
        }
    }
}