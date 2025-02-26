using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    // Update viene chiamato una volta per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Se il pulsante sinistro del mouse viene cliccato
        {
            // Ottieni la posizione del click nel mondo
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 clickPosition = hit.point;

                // Trova il waypoint più vicino al giocatore
                Waypoint startWaypoint = WaypointManager.Instance.FindClosestWaypoint(transform.position);

                // Trova il waypoint più vicino al click
                Waypoint endWaypoint = WaypointManager.Instance.FindClosestWaypoint(clickPosition);

                //
                if (startWaypoint == null || endWaypoint == null)
                    return;

                //
                Debug.Log("WP di inizio: " + startWaypoint.name + " e di fine: " + endWaypoint.name);

                //
                List<Waypoint> waypoints = BuildPath(startWaypoint, endWaypoint);

                // TODO: Personaggio che deve andare da A a B

            }
        }
    }

    public List<Waypoint> BuildPath(Waypoint startWaypoint, Waypoint endWaypoint)
    {
        // Lista temporanea per memorizzare il percorso
        List<Waypoint> path = new();

        // Cammina all'indietro dal waypoint iniziale, se ci sono waypoint precedenti
        Waypoint currentWaypoint = startWaypoint;
        while (currentWaypoint.PrevWaypoint != null)
        {
            currentWaypoint = currentWaypoint.PrevWaypoint;
        }

        // Da qui, cammina in avanti lungo la catena dei waypoint fino a raggiungere l'endWaypoint
        while (currentWaypoint != null)
        {
            // Aggiungi il waypoint corrente al percorso
            path.Add(currentWaypoint);

            // Se abbiamo raggiunto l'endWaypoint, fermiamoci
            if (currentWaypoint == endWaypoint)
                break;

            // Vai al prossimo waypoint
            currentWaypoint = currentWaypoint.NextWaypoint;
        }

        // Stampa il percorso per debug
        foreach (Waypoint waypoint in path)
        {
            Debug.Log("Passando per waypoint: " + waypoint.name);
        }

        //
        return path;
    }
}
