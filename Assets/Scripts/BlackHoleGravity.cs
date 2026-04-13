using UnityEngine;

public class BlackHoleGravity : MonoBehaviour
{
    [SerializeField] private Rigidbody2D shipRigidbody;
    [SerializeField] private MeteorSpawner meteorSpawner;
    [SerializeField] private float gravityStrength = 500f;
    [SerializeField] private float minDistance = 1f;

    private void FixedUpdate()
    {
        ApplyGravity(shipRigidbody);

        if (meteorSpawner == null) return;
        foreach (Meteor meteor in meteorSpawner.GetActiveMeteors())
        {
            if (meteor != null)
                ApplyGravity(meteor.GetComponent<Rigidbody2D>());
        }
    }

    private void ApplyGravity(Rigidbody2D target)
    {
        if (target == null) return;
        Vector2 direction = (Vector2)transform.position - target.position;
        float distance = Mathf.Max(direction.magnitude, minDistance);
        //originally inverse square but it just wasnt good for gameplay, so now its just inverse linear
        float forceMagnitude = gravityStrength / (distance);
        target.AddForce(direction.normalized * forceMagnitude);
    }
}
