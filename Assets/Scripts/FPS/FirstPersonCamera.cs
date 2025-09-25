using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivityX = 100f;
    public float mouseSensitivityY = 100f;

    [Header("Camera")]
    public Transform playerBody;
    private float xRotation = 0f;

    void Update()
    {
        // MouseX, MouseY, StickAnalogicoDestroX, Y => azione

        // Ottieni input del mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;

        // Calcola la rotazione verticale (asse X)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Applica la rotazione alla camera (solo su X)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Ruota il corpo del player (asse Y)
        // (0, 1, 0) * 0.2 = (0, 0.2, 0)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}