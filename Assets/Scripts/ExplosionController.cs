using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;

public class ExplosionController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;

    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private GameObject explosionEffectPrefab;

    [Header("Attack Range")]
    [SerializeField] private Vector2 attackRangeSize = new Vector2(20f, 20f);

    [Header("Laser")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserTravelTime = 0.5f;
    [SerializeField] private float attackDelay = 0.2f;
    [SerializeField] private float attackCooldown = 1f;

    private float lastAttackTime = -Mathf.Infinity;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    public void OnAttack(InputValue value)
    {
        Debug.Log("Attack input received: " + value);
        if (!value.isPressed) return;
        Debug.Log("Processing attack...");
        if (ShipController.instance == null || !ShipController.instance.WeaponsEnabled) return;
        Debug.Log("Weapons enabled, executing attack...");

        if (Time.time < lastAttackTime + attackCooldown) return;

        Vector2 worldPos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector2 offset = worldPos - (Vector2)transform.position;
        if (Mathf.Abs(offset.x) > attackRangeSize.x / 2f || Mathf.Abs(offset.y) > attackRangeSize.y / 2f) return;

        lastAttackTime = Time.time;
        StartCoroutine(AttackSequence(worldPos));
    }

    private IEnumerator AttackSequence(Vector2 targetPos)
    {
        yield return new WaitForSeconds(attackDelay);

        Vector2 startPos = transform.position;
        GameObject laser = null;
        if (laserPrefab != null)
        {
            Vector2 dir = targetPos - startPos;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
            laser = Instantiate(laserPrefab, startPos, Quaternion.Euler(0f, 0f, angle));
        }

        float elapsed = 0f;
        while (elapsed < laserTravelTime)
        {
            elapsed += Time.deltaTime;
            Vector2 pos = Vector2.Lerp(startPos, targetPos, elapsed / laserTravelTime);
            if (laser != null) laser.transform.position = pos;
            yield return null;
        }

        if (laser != null) Destroy(laser);

        if (explosionEffectPrefab != null)
            Instantiate(explosionEffectPrefab, targetPos, Quaternion.identity);

        Collider2D[] hits = Physics2D.OverlapCircleAll(targetPos, explosionRadius);
        foreach (Collider2D hit in hits)
        {
            Meteor meteor = hit.GetComponent<Meteor>();
            if (meteor != null)
                meteor.Explode();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, attackRangeSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
