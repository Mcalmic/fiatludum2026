using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour
{
    public static ShipController instance;

    [SerializeField] private PathDefinition path;
    [SerializeField] private float shipSpeed = 5f;
    [SerializeField] private float thrustForce = 50f;
    [SerializeField] private float waypointReachDistance = 0.5f;
    [SerializeField] private float autopilotSteerSpeed = 5f;
    [SerializeField] private float manualSteerSpeed = 5f;
    [SerializeField] private float meteorDamage = 20f;

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

    //STUFF ANDREW ADDED FOR UI TO WORK:
    [SerializeField] private RectTransform uiViewRect;

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
        if (shieldObject != null) shieldObject.SetActive(shieldEnabled);
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
        RefreshModeButtonColors();
    }

    public void ToggleAutopilot()
    {
        autopilotEnabled = !autopilotEnabled;
        navigationEnabled = false;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Meteor>() != null)
        {   
            if(collision.gameObject.GetComponent<Meteor>().hasHitPlayer) return;
            collision.gameObject.GetComponent<Meteor>().hasHitPlayer = true;
            GameManager.instance?.DrainBattery(meteorDamage);
        }
    }

    public void OnClick(InputValue value)
    {
        // if (autopilotEnabled) return;
        // if (!value.isPressed) return;
        // if (!navigationEnabled) return;

        // Vector2 screenPos = Mouse.current.position.ReadValue();
        // clickTarget = cam.ScreenToWorldPoint(screenPos);
        // hasClickTarget = true;

        if (autopilotEnabled) return;
        if (!value.isPressed) return;
        if (!navigationEnabled) return;

        Vector2 screenPos = Mouse.current.position.ReadValue();

        // 1. Convert the screen click into a local coordinate inside your UI element
        // Note: If your Canvas is set to "Screen Space - Camera", replace 'null' with your UI Camera.
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(uiViewRect, screenPos, null, out Vector2 localPoint))
        {
            // 2. Check if the click was actually inside the boundaries of the UI image
            if (uiViewRect.rect.Contains(localPoint))
            {
                // 3. Normalize the coordinate to a 0.0 - 1.0 scale (Viewport Space)
                // This math works regardless of where your UI pivot is set.
                float normalizedX = (localPoint.x - uiViewRect.rect.x) / uiViewRect.rect.width;
                float normalizedY = (localPoint.y - uiViewRect.rect.y) / uiViewRect.rect.height;
                Vector2 viewportPoint = new Vector2(normalizedX, normalizedY);

                // 4. Use the Camera's ViewportToWorldPoint instead of ScreenToWorldPoint
                clickTarget = cam.ViewportToWorldPoint(viewportPoint);
                
                // Optional: Flatten the Z axis if this is a 2D game
                //clickTarget.z = 0f; 
                
                hasClickTarget = true;
            }
        }
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

        if (rb.linearVelocity.magnitude < shipSpeed)
            rb.AddForce((Vector2)transform.up * thrustForce);
    }

    private void FollowPath()
    {
        Vector3 target = path.GetWaypoint(currentWaypointIndex);
        SteerToward(target, autopilotSteerSpeed);

        if (rb.linearVelocity.magnitude < shipSpeed)
            rb.AddForce((Vector2)transform.up * thrustForce);

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
