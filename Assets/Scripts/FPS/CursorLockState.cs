using UnityEngine;

public class CursorLockState : MonoBehaviour
{
    void Start()
    {
        // Blocca il cursore al centro dello schermo
        Cursor.lockState = CursorLockMode.Locked;
    }
}