using UnityEngine;

public class TopDownCameraAndroid : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 0.1f; // Speed of zooming
    public float minZoom = 10f; // Minimum zoom distance
    public float maxZoom = 50f; // Maximum zoom distance

    [Header("Pan Settings")]
    public float panSpeed = 0.5f; // Speed of panning
    public Vector2 panLimit = new Vector2(50f, 50f); // Limits for panning

    private Camera cam;
    private Vector2 lastPanPosition; // Last position for panning
    private int panFingerId; // Finger ID used for panning

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            HandlePan();
        }
        else if (Input.touchCount == 2)
        {
            HandleZoom();
        }
    }

    private void HandlePan()
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            // Capture the initial touch position
            lastPanPosition = touch.position;
            panFingerId = touch.fingerId;
        }
        else if (touch.phase == TouchPhase.Moved && touch.fingerId == panFingerId)
        {
            // Calculate the touch delta and move the camera
            Vector2 delta = touch.position - lastPanPosition;
            Vector3 move = new Vector3(-delta.x * panSpeed * Time.deltaTime, 0, -delta.y * panSpeed * Time.deltaTime);

            transform.position += move;

            // Clamp the camera position to stay within bounds
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, -panLimit.x, panLimit.x),
                transform.position.y,
                Mathf.Clamp(transform.position.z, -panLimit.y, panLimit.y)
            );

            lastPanPosition = touch.position; // Update the last pan position
        }
    }

    private void HandleZoom()
    {
        if (Input.touchCount == 2)
        {
            // Get the two touches
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Calculate the previous and current distances between the two touches
            float prevDistance = (touch1.position - touch1.deltaPosition - (touch2.position - touch2.deltaPosition)).magnitude;
            float currentDistance = (touch1.position - touch2.position).magnitude;

            // Calculate the distance delta
            float distanceDelta = prevDistance - currentDistance;

            // Adjust the field of view (FOV) to zoom in/out
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView + distanceDelta * zoomSpeed, minZoom, maxZoom);

            Debug.Log($"Zoom detected. Current FOV: {cam.fieldOfView}");
        }
    }

}
