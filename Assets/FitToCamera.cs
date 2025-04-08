using UnityEngine;

[RequireComponent(typeof(Renderer))]
[ExecuteAlways] // Makes this run in both Play and Edit Mode
public class FitToCamera : MonoBehaviour
{
    public Camera targetCamera;

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        FitToCameraCanvas();
    }

    void FitToCameraCanvas()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogWarning("No Renderer found on this object.");
            return;
        }

        // Get object size in local space
        Vector3 objectSize = renderer.bounds.size;

        // Get camera height and width in world units
        float cameraHeight = 2f * targetCamera.orthographicSize;
        float cameraWidth = cameraHeight * targetCamera.aspect;

        // Calculate scale factors
        Vector3 scale = transform.localScale;
        scale.x = cameraWidth / objectSize.x;
        scale.y = cameraHeight / objectSize.y;

        transform.localScale = scale;
    }
}