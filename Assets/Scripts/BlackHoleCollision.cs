using UnityEngine;

public class BlackHoleCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Meteor meteor = other.GetComponent<Meteor>();
        if (meteor != null)
            meteor.Explode();

        ShipController ship = other.GetComponent<ShipController>();
        if (ship != null)
        {
            SceneSwitcher.instance.endingType = "bhole";
            SceneSwitcher.instance.SwitchToScene("Ending");
        }
    }
}
