using UnityEngine;
using UnityEngine.InputSystem;

public class CarPinchZoom : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 0.005f;
    [SerializeField] private float minimumZoom = 0.5f;
    [SerializeField] private float maximumZoom = 2.5f;

    private Vector3 originalScale;
    private float zoomMultiplier = 1f;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (Touchscreen.current == null)
            return;

        var touch0 = Touchscreen.current.touches[0];
        var touch1 = Touchscreen.current.touches[1];

        if (!touch0.press.isPressed ||
            !touch1.press.isPressed)
            return;

        Vector2 current0 = touch0.position.ReadValue();
        Vector2 current1 = touch1.position.ReadValue();

        Vector2 previous0 =
            current0 - touch0.delta.ReadValue();

        Vector2 previous1 =
            current1 - touch1.delta.ReadValue();

        float currentDistance =
            Vector2.Distance(current0, current1);

        float previousDistance =
            Vector2.Distance(previous0, previous1);

        float difference =
            currentDistance - previousDistance;

        zoomMultiplier += difference * zoomSpeed;

        zoomMultiplier = Mathf.Clamp(
            zoomMultiplier,
            minimumZoom,
            maximumZoom
        );

        transform.localScale =
            originalScale * zoomMultiplier;
    }
}