using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint PrevWaypoint;
    public Waypoint NextWaypoint;
    [HideInInspector] public Vector3 Position => transform.position;

    private Vector3 offset = new(0, -0.25f, 0);

    void OnDrawGizmos()
    {
        if (NextWaypoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(NextWaypoint.Position - offset, transform.position - offset);
            Gizmos.color = Color.white;
        }

        if (PrevWaypoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(PrevWaypoint.Position + offset, transform.position + offset);
            Gizmos.color = Color.white;
        }
    }
}
