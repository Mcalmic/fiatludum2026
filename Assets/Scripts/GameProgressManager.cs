using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager instance;

    [SerializeField] private Transform finalDestination;
    [SerializeField] private float arrivalDistance = 1f;
    [SerializeField] private GameObject completionScreen;
    [SerializeField] private ShipController shipController;

    private bool arrived = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (arrived) return;
        if (shipController == null || finalDestination == null) return;

        if (GetDistanceToDestination() <= arrivalDistance)
        {
            arrived = true;
            SceneSwitcher.instance.endingType = "good";
            SceneSwitcher.instance.SwitchToScene("Ending");
        }
    }

    public float GetDistanceToDestination()
    {
        if (shipController == null || finalDestination == null) return 0f;
        return Vector2.Distance(shipController.transform.position, finalDestination.position);
    }
}
