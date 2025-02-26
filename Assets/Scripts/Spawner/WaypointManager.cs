using UnityEngine;

public class WaypointManager : Singleton<WaypointManager>
{
    public Waypoint[] waypoints; // Array di tutti i waypoint nella scena

    void Start()
    {
        waypoints = FindObjectsByType<Waypoint>(FindObjectsSortMode.InstanceID); // Trova tutti i waypoint nella scena
    }

    // Metodo per trovare il waypoint più vicino dato un punto cliccato
    public Waypoint FindClosestWaypoint(Vector3 clickPosition)
    {
        Waypoint closestWaypoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (Waypoint waypoint in waypoints)
        {
            float distance = Vector3.Distance(clickPosition, waypoint.Position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestWaypoint = waypoint;
            }
        }

        return closestWaypoint;
    }
}