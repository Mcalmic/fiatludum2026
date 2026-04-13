using UnityEngine;

public class Meteor : MonoBehaviour
{

    private Rigidbody2D rb;
    private MeteorSpawner spawner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    public void Initialize(MeteorSpawner s, Vector2 velocity)
    {
        spawner = s;
        rb.linearVelocity = velocity;
    }

    public void Explode()
    {
        spawner?.OnMeteorDestroyed(this);
        Destroy(gameObject);
    }
}
