using UnityEngine;
using UnityEngine.InputSystem;

public class ZoomInOut : MonoBehaviour
{
    public enum CameraMode { Following, FullView }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Camera cam;

    [Header("Follow")]
    [SerializeField] private float followSmoothTime = 0.15f;
    [SerializeField] private float defaultCameraFOVSize = 10f;

    [Header("Zoom")]
    [SerializeField] private float zoomStep = 2f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 40f;
    [SerializeField] private float zoomSmoothTime = 0.1f;

    [Header("Full View")]
    [SerializeField] private float fullViewFOVSize = 45f;
    [SerializeField] private Vector3 fullViewPosition;

    private CameraMode mode = CameraMode.Following;
    private Vector3 followVelocity;
    private float targetFOVSize;
    private float FOVSizeVelocity;
    private Vector3 smoothTargetPos;

    private void Awake()
    {
        if (cam == null) cam = GetComponent<Camera>();
        targetFOVSize = defaultCameraFOVSize;
        cam.orthographicSize = defaultCameraFOVSize;
        smoothTargetPos = cam.transform.position;
        smoothTargetPos.z = -10f;
    }

    private void LateUpdate()
    {
        if (mode == CameraMode.Following && player != null)
            smoothTargetPos = new Vector3(player.position.x, player.position.y, -10f);

        Vector3 newPos = Vector3.SmoothDamp(cam.transform.position, smoothTargetPos, ref followVelocity, followSmoothTime);
        newPos.z = -10f;
        cam.transform.position = newPos;

        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetFOVSize, ref FOVSizeVelocity, zoomSmoothTime);
    }

    public void OnZoom(InputValue value)
    {
        if (mode == CameraMode.FullView) return;

        float scroll = value.Get<Vector2>().y;
        if (scroll > 0f)
            targetFOVSize = Mathf.Max(targetFOVSize - zoomStep, minZoom);
        else if (scroll < 0f)
            targetFOVSize = Mathf.Min(targetFOVSize + zoomStep, maxZoom);
    }

    public void FullView()
    {
        if (mode == CameraMode.Following)
        {
            mode = CameraMode.FullView;
            smoothTargetPos = new Vector3(fullViewPosition.x, fullViewPosition.y, -10f);
            targetFOVSize = fullViewFOVSize;
        }
        else
        {
            mode = CameraMode.Following;
            targetFOVSize = defaultCameraFOVSize;
        }
    }
}
