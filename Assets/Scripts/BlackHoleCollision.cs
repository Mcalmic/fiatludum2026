using UnityEngine;

public class BlackHoleCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Meteor meteor = other.GetComponent<Meteor>();
        if (meteor != null)
            meteor.Explode();
    }
}
