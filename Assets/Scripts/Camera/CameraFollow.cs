using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Il personaggio da seguire
    public float smoothSpeed = 0.125f; // Velocità di transizione per il movimento della camera
    public Vector3 offset; // Distanza tra la camera e il personaggio
    public Vector3 negativeOffset;
    private Vector3 desiredPosition = Vector3.zero;

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        //
        if (moveInput > 0)
        {
            // Posizione desiderata della camera con offset
            desiredPosition = target.position + negativeOffset;
        }
        else
        {
            // Posizione desiderata della camera con offset
            desiredPosition = target.position + offset;
        }
        
        // Movimento liscio della camera verso la posizione desiderata
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Imposta la posizione della camera
        transform.position = smoothedPosition;
    }
}