using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Il personaggio da seguire
    public float smoothSpeed = 0.125f; // Velocità di transizione per il movimento della camera
    public Vector3 offset; // Distanza tra la camera e il personaggio
    public Vector3 negativeOffset;
    private Vector3 desiredPosition;
    private Rigidbody2D rb;

    void Start()
    {
        rb = target.gameObject.GetComponent<Rigidbody2D>();
        desiredPosition = (Vector3)rb.position + offset;
    }

    void LateUpdate()
    {
        // 
        if (target == null || rb == null)
        {
            Debug.LogWarning("Assegna un Target o RB al tuo target.");
            return;
        }

        //
        float moveInput = Input.GetAxisRaw("Horizontal");

        //
        /*if (moveInput > 0)
        {
            // Posizione desiderata della camera con offset
            desiredPosition = target.position + negativeOffset;
        }
        else
        {
            // Posizione desiderata della camera con offset
            desiredPosition = target.position + offset;
        }*/

        //
        desiredPosition = (Vector3)rb.position + offset;
        
        // Movimento liscio della camera verso la posizione desiderata
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Imposta la posizione della camera
        transform.position = smoothedPosition;
    }
}