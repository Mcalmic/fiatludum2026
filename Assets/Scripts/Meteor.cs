using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour
{

    private Rigidbody2D rb;
    private MeteorSpawner spawner;
    public bool hasHitPlayer = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    public void Initialize(MeteorSpawner s, Vector2 velocity)
    {
        spawner = s;
        rb.linearVelocity = velocity;
        StartCoroutine(LifetimeCoroutine());
    }

    private IEnumerator LifetimeCoroutine()
    {
        yield return new WaitForSeconds(20f);
        Explode();
    }

    public void Explode()
    {
        spawner?.OnMeteorDestroyed(this);
        Destroy(gameObject);
    }
}
