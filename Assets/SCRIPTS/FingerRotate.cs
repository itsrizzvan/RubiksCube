using UnityEngine;
using UnityEngine.UI;

public class FingerRotate : MonoBehaviour
{
    [Header("Mode Control")]
    [Tooltip("If true, finger touches rotate the cube. If false, the cube strictly follows AR tracking without manual movement.")]
    public bool isAnalyzeMode = false;

    [Header("Rotation Settings (Analyze Mode Only)")]
    [SerializeField] private float rotationSpeed = 0.2f;
    [SerializeField] private float damping = 5f;

    [Header("UI Reference")]
    [SerializeField] private Image analyzeButtonImage; // Visual indicator when analyze mode is toggled
    [SerializeField] private Color activeColor = Color.green;
    [SerializeField] private Color defaultColor = Color.white;

    private Vector2 touchDelta;
    private Vector2 currentVelocity;
    private Touch touch;
    private Camera arCamera;

    void Start()
    {
        // Automatically fetch the active AR Camera tagged MainCamera
        arCamera = Camera.main;
    }

    void Update()
    {
        // ONLY process touch gestures when in Analyze Mode
        if (isAnalyzeMode)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                // Prevent touch input when tapping on UI elements (like the analyze button)
                if (UnityEngine.EventSystems.EventSystem.current != null &&
                    UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    return;
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    touchDelta = touch.deltaPosition;

                    // Calculate smooth rotation velocity from finger movement
                    currentVelocity = touchDelta * rotationSpeed;
                }
            }

            // Apply inertia and smooth rotation while in Analyze Mode
            if (currentVelocity.sqrMagnitude > 0.001f)
            {
                float rotY = -currentVelocity.x;
                float rotX = currentVelocity.y;

                transform.Rotate(Vector3.up, rotY, Space.World);

                if (arCamera != null)
                {
                    transform.Rotate(arCamera.transform.right, rotX, Space.World);
                }

                // Smoothly coast to a stop when finger is lifted
                currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, Time.deltaTime * damping);
            }
        }
    }

    /// <summary>
    /// Attach this public method to your UI Button's OnClick event!
    /// </summary>
    public void ToggleAnalyzeMode()
    {
        isAnalyzeMode = !isAnalyzeMode;
        
        // Zero out velocity so it doesn't spin off if re-enabled abruptly
        currentVelocity = Vector2.zero;

        // Visual tint update for UI Feedback
        if (analyzeButtonImage != null)
        {
            analyzeButtonImage.color = isAnalyzeMode ? activeColor : defaultColor;
        }

        Debug.Log("Analyze Mode active: " + isAnalyzeMode);
    }
}