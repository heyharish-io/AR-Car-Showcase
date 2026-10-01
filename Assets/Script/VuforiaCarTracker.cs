using UnityEngine;
using Vuforia;

public class VuforiaCarTracker : MonoBehaviour
{
    [SerializeField] private ObserverBehaviour imageTarget;
    [SerializeField] private GameObject carPrefab;

    [SerializeField] private Vector3 localPosition = Vector3.zero;
    [SerializeField] private Vector3 localRotation = Vector3.zero;
    [SerializeField] private Vector3 localScale = Vector3.one;

    [SerializeField] private bool hideWhenTargetIsLost = true;

    private GameObject carInstance;

    private void Awake()
    {
        if (imageTarget == null)
            imageTarget = GetComponent<ObserverBehaviour>();
    }

    private void Start()
    {
        if (imageTarget == null)
        {
            Debug.LogError("VuforiaCarTracker: Image Target is missing.");
            return;
        }

        if (carPrefab == null)
        {
            Debug.LogError("VuforiaCarTracker: Car Prefab is missing.");
            return;
        }

        carInstance = Instantiate(carPrefab, imageTarget.transform);

        carInstance.name = "TrackedCar_Runtime";

        carInstance.transform.localPosition = localPosition;
        carInstance.transform.localRotation = Quaternion.Euler(localRotation);
        carInstance.transform.localScale = localScale;

        carInstance.SetActive(false);
    }

    private void Update()
    {
        if (imageTarget == null || carInstance == null)
            return;

        TargetStatus status = imageTarget.TargetStatus;

        bool tracked = status.Status == Status.TRACKED;

        if (tracked)
        {
            if (!carInstance.activeSelf)
                carInstance.SetActive(true);
        }
        else if (hideWhenTargetIsLost)
        {
            if (carInstance.activeSelf)
                carInstance.SetActive(false);
        }
    }
}