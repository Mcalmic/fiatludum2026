using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour
{
    public static ShipController instance;

    [SerializeField] private PathDefinition path;
    [SerializeField] private float shipSpeed = 5f;
    [SerializeField] private float waypointReachDistance = 0.5f;
    [SerializeField] private float autopilotSteerSpeed = 5f;
    [SerializeField] private float manualSteerSpeed = 5f;

    [Header("Click To Move")]
    [SerializeField] private Camera cam;
    [SerializeField] private LineRenderer clickLine;

    [Header("Mode Toggles")]
    [SerializeField] private bool weaponsEnabled = false;
    [SerializeField] private bool navigationEnabled = true;
    [SerializeField] private bool autopilotEnabled = false;
    [SerializeField] private Image weaponsButtonImage;
    [SerializeField] private Image navigationButtonImage;
    [SerializeField] private Image autopilotButtonImage;
    [SerializeField] private Image shieldButtonImage;


    [Header("Shield")]
    [SerializeField] private GameObject shieldObject;

    public bool WeaponsEnabled => weaponsEnabled;
    public bool NavigationEnabled => navigationEnabled;

    private Rigidbody2D rb;
    private int currentWaypointIndex = 0;
    private Vector2 clickTarget = Vector2.zero;
    private bool hasClickTarget = false;
    private bool shieldEnabled = false;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        if (cam == null) cam = Camera.main;
    }

    private void Start()
    {
        if (shieldObject != null) shieldObject.SetActive(false);
        RefreshModeButtonColors();
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

    public void ToggleWeapons()
    {
        weaponsEnabled = !weaponsEnabled;
        navigationEnabled = false;
        shieldEnabled = false;
        RefreshModeButtonColors();
    }

    public void ToggleNavigation()
    {
        navigationEnabled = !navigationEnabled;
        weaponsEnabled = false;
        autopilotEnabled = false;
        RefreshModeButtonColors();
    }

    public void ToggleShield()
    {
        shieldEnabled = !shieldEnabled;
        weaponsEnabled = false;
        if (shieldObject != null) shieldObject.SetActive(shieldEnabled);
        if (shieldButtonImage != null)
            shieldButtonImage.color = shieldEnabled ? Color.green : Color.white;
        RefreshModeButtonColors();
    }

    public void ToggleAutopilot()
    {
        autopilotEnabled = !autopilotEnabled;
        navigationEnabled = false;
        weaponsEnabled = false;
        shieldEnabled = false;
        if (shieldObject != null) shieldObject.SetActive(false);
        hasClickTarget = false;
        clickLine.enabled = false;
        RefreshModeButtonColors();
    }

    private void RefreshModeButtonColors()
    {
        if (weaponsButtonImage != null)
            weaponsButtonImage.color = weaponsEnabled ? Color.green : Color.white;
        if (navigationButtonImage != null)
            navigationButtonImage.color = navigationEnabled ? Color.green : Color.white;
        if (autopilotButtonImage != null)
            autopilotButtonImage.color = autopilotEnabled ? Color.green : Color.white;
        if (shieldButtonImage != null)
            shieldButtonImage.color = shieldEnabled ? Color.green : Color.white;
    }

    public void OnClick(InputValue value)
    {
        if (autopilotEnabled) return;
        if (!value.isPressed) return;
        if (!navigationEnabled) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

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
