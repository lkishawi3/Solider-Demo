using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 8f;
    public float height = 4f;
    public float smoothTime = 0.5f;
    public float threshold = 20f;
    
    private Vector3 currentVelocity;
    private Vector3 desiredPosition;

    void Start()
    {
        desiredPosition = transform.position;
    }

    void LateUpdate()
    {
        if (!target) return;

        // Calculate desired position based on mouse input
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distance;
        Vector3 targetPos = target.position;
        Vector3 followPos = Camera.main.ScreenToWorldPoint(mousePos);
        followPos.y = targetPos.y + height;

        // Check if mouse movement exceeds threshold
        if (Vector3.Distance(desiredPosition, followPos) > threshold)
        {
            desiredPosition = followPos;
        }

        // Calculate camera target position relative to the character
        Vector3 cameraTargetPos = targetPos - target.forward * distance;
        cameraTargetPos.y = targetPos.y + height;

        // Smoothly move to the camera target position
        transform.position = Vector3.SmoothDamp(
            transform.position,
            cameraTargetPos,
            ref currentVelocity,
            smoothTime
        );

        // Look at the character with offset
        Vector3 lookAtPos = targetPos + Vector3.up * 1f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAtPos - transform.position), smoothTime);
    }
}