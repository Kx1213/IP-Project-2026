using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Transform cam;

    void Start()
    {
        if (Camera.main != null)
            cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (cam == null) return;

        Vector3 direction = transform.position - cam.position;
        direction.y = 0f; // optional: keeps UI upright

        transform.rotation = Quaternion.LookRotation(direction);
    }
}
