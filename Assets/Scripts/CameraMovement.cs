using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;  // The target (character) the camera will follow
    public float smoothTime = 0.3f;  // Time it takes to smooth the camera movement
    public Vector3 offset = new Vector3(0, 0, -10);  // Offset from the target (usually for keeping the camera behind in 2D)

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target != null)
        {
            // Target position with offset
            Vector3 targetPosition = target.position + offset;

            // Smoothly move the camera towards the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}

