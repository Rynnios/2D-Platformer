using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private float originalOrthoSize;
    private Transform originalFollow;
    private Transform originalLookAt;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        // Save initial camera properties
        originalOrthoSize = virtualCamera.m_Lens.OrthographicSize;
        originalFollow = virtualCamera.Follow;
        originalLookAt = virtualCamera.LookAt;

        // Save initial camera position and rotation
        originalPosition = virtualCamera.transform.position;
        originalRotation = virtualCamera.transform.rotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Change camera properties for zoom out and lock
            virtualCamera.m_Lens.OrthographicSize = 10;
            virtualCamera.Follow = null;
            virtualCamera.LookAt = null;

            // Move and lock camera to trigger's x and y position, keep original z position
            Vector3 triggerPosition = transform.position;
            virtualCamera.transform.position = new Vector3(triggerPosition.x, triggerPosition.y, originalPosition.z);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Restore original camera properties
            virtualCamera.m_Lens.OrthographicSize = originalOrthoSize;
            virtualCamera.Follow = originalFollow;
            virtualCamera.LookAt = originalLookAt;

            // Restore original camera position and rotation
            virtualCamera.transform.position = originalPosition;
            virtualCamera.transform.rotation = originalRotation;
        }
    }
}