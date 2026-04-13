using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ExplosionController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;

    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private GameObject explosionEffectPrefab;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    public void OnAttack(InputValue value)
    {
        if (!value.isPressed) return;
        if (ShipController.instance == null || !ShipController.instance.WeaponsEnabled) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        Debug.Log("Explosion triggered!");

        Vector2 worldPos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (explosionEffectPrefab != null)
            Instantiate(explosionEffectPrefab, worldPos, Quaternion.identity);

        Collider2D[] hits = Physics2D.OverlapCircleAll(worldPos, explosionRadius);
        foreach (Collider2D hit in hits)
        {
            Meteor meteor = hit.GetComponent<Meteor>();
            if (meteor != null)
                meteor.Explode();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
