using UnityEngine;

public class PathDefinition : MonoBehaviour
{
    [SerializeField] private Vector3[] waypoints = new Vector3[0];
    [SerializeField] private float gizmoNodeRadius = 0.3f;

    [Header("Dots")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private float dotSpacing = 1f;

    private void Start()
    {
        SpawnDots();
    }

    private void SpawnDots()
    {

        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Vector3 start = waypoints[i];
            Vector3 end = waypoints[i + 1];
            float segmentLength = Vector3.Distance(start, end);
            int dotCount = Mathf.FloorToInt(segmentLength / dotSpacing);

            for (int j = 0; j <= dotCount; j++)
            {
                Vector3 pos = Vector3.Lerp(start, end, j / (float) dotCount);
                Instantiate(dotPrefab, pos, Quaternion.identity, transform);
            }
        }
    }

    public Vector3 GetWaypoint(int index)
    {
        return waypoints[index];
    }

    public int GetWaypointCount()
    {
        return waypoints == null ? 0 : waypoints.Length;
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Gizmos.color = Color.white;

        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.DrawWireSphere(waypoints[i], gizmoNodeRadius);

            if (i < waypoints.Length - 1)
                Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
        }
    }
}
