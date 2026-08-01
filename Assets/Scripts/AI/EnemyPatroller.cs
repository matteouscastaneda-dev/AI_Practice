using UnityEngine;

public class EnemyPatroller : EnemyBase
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float arriveDistance = 0.5f;
    [SerializeField] private float arrowSize = 0.5f;

    private int currentWaypointIndex;

    /// <summary>
    /// gets waypoints from the spawner
    /// </summary>
    /// <param name="newWaypoints"></param>
    public void SetWaypoints(GameObject[] newWaypoints)
    {
        waypoints = newWaypoints;
        currentWaypointIndex = 0;
    }

    /// <summary>
    /// Walks to the current waypoint
    /// </summary>
    protected override void DoDefaultBehavior()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            return;
        }

        agent.updateRotation = true;

        Vector3 target = waypoints[currentWaypointIndex].transform.position;

        if (Vector3.Distance(transform.position, target) <= arriveDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            target = waypoints[currentWaypointIndex].transform.position;
        }

        agent.SetDestination(target);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        DrawWaypointPath();
    }

    private void DrawWaypointPath()
    {
        if (waypoints == null || waypoints.Length < 2)
        {
            return;
        }

        Gizmos.color = Color.blue;

        for (int i = 0; i < waypoints.Length; i++)
        {
            int nextIndex = (i + 1) % waypoints.Length;

            if (waypoints[i] == null || waypoints[nextIndex] == null)
            {
                continue;
            }

            Vector3 currentWaypoint = waypoints[i].transform.position;
            Vector3 nextWaypoint = waypoints[nextIndex].transform.position;

            Gizmos.DrawLine(currentWaypoint, nextWaypoint);

            DrawArrowHead(currentWaypoint, nextWaypoint, arrowSize);
        }
    }

    private void DrawArrowHead(Vector3 from, Vector3 to, float size)
    {
        Vector3 direction = (to - from).normalized;
        Vector3 midpoint = from + (to - from) * 0.5f;

        Vector3 right = Vector3.Cross(direction, Vector3.up).normalized * size;
        Vector3 up = Vector3.Cross(right, direction).normalized * size;

        Vector3 arrowTip = midpoint + direction * size;

        Gizmos.DrawLine(midpoint + right, arrowTip);
        Gizmos.DrawLine(midpoint - right, arrowTip);
        Gizmos.DrawLine(midpoint + up, arrowTip);
        Gizmos.DrawLine(midpoint - up, arrowTip);
    }
}