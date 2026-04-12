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

    public void Initialize(MeteorSpawner s)
    {
        spawner = s;
    }

    public void Explode()
    {
        //just now commented this out bc it was causing compiler errors :333
        //spawner.OnMeteorDestroyed();
        Destroy(gameObject);
    }
}
