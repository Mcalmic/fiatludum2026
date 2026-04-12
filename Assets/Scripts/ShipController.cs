using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour
{
    [SerializeField] private PathDefinition path;
    [SerializeField] private float shipSpeed = 5f;
    [SerializeField] private float waypointReachDistance = 0.5f;
    [SerializeField] private float autopilotSteerSpeed = 5f;
    [SerializeField] private float manualSteerSpeed = 5f;

    [Header("Click To Move")]
    [SerializeField] private Camera cam;
    [SerializeField] private LineRenderer clickLine;

    private Rigidbody2D rb;
    [SerializeField] private bool autopilotEnabled = false;
    private int currentWaypointIndex = 0;
    private Vector2 clickTarget = Vector2.zero;
    private bool hasClickTarget = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        if (cam == null) cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (!autopilotEnabled && hasClickTarget)
        {
            clickLine.enabled = true;
            clickLine.SetPosition(0, transform.position);
            clickLine.SetPosition(1, clickTarget);
        }
        else
        {
            clickLine.enabled = false;
        }
    }

    public void EnableAutopilot(bool value)
    {
        autopilotEnabled = value;
        hasClickTarget = false;
        clickLine.enabled = false;
    }

    public void OnClick(InputValue value)
    {
        if (autopilotEnabled) return;
        if (!value.isPressed) return;

        Vector2 screenPos = Mouse.current.position.ReadValue();
        clickTarget = cam.ScreenToWorldPoint(screenPos);
        hasClickTarget = true;
    }

    private void FixedUpdate()
    {
        if (autopilotEnabled)
            FollowPath();
        else
            MoveForward();
    }

    private void MoveForward()
    {
        if (hasClickTarget)
        {
            SteerToward(clickTarget, manualSteerSpeed);

            if (Vector2.Distance(rb.position, clickTarget) <= waypointReachDistance)
                hasClickTarget = false;
        }

        rb.linearVelocity = (Vector2)transform.up * shipSpeed;
    }

    private void FollowPath()
    {
        Vector3 target = path.GetWaypoint(currentWaypointIndex);
        SteerToward(target, autopilotSteerSpeed);
        rb.linearVelocity = (Vector2)transform.up * shipSpeed;

        if (Vector2.Distance(rb.position, target) <= waypointReachDistance)
        {
            if (currentWaypointIndex < path.GetWaypointCount() - 1)
                currentWaypointIndex++;
        }
    }

    private void SteerToward(Vector2 target, float steerSpeed)
    {
        Vector2 direction = (target - rb.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float smoothedAngle = Mathf.LerpAngle(rb.rotation, angle, steerSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(smoothedAngle);
    }
}
