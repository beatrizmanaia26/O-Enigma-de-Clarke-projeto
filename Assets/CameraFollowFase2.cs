using UnityEngine;

public class CameraFollowFase2 : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public float minX = -1f;
    public float maxX = 53.56915f;
    public float fixedY = 0f;

    void LateUpdate()
    {
        if (target == null) return;

        float desiredX = target.position.x + offset.x;
        float clampedX = Mathf.Clamp(desiredX, minX, maxX);

        Vector3 desiredPosition = new Vector3(
            clampedX,
            fixedY,
            -10f
        );

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}