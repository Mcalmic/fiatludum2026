using UnityEngine;

public class BlackHoleGravity : MonoBehaviour
{
    [SerializeField] private Rigidbody2D shipRigidbody;
    [SerializeField] private float gravityStrength = 500f;
    [SerializeField] private float minDistance = 1f;

    private void FixedUpdate()
    {
        //maybe list of objects that are affected by gravity?

        Vector2 direction = (Vector2)transform.position - shipRigidbody.position;
        float distance = Mathf.Max(direction.magnitude, minDistance);
        float forceMagnitude = gravityStrength / (distance * distance);

        shipRigidbody.AddForce(direction.normalized * forceMagnitude);
    }
}
