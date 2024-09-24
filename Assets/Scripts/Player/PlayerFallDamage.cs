using UnityEngine;

public class PlayerFallDamage : MonoBehaviour
{
    private float firstY;
    private float lastY;
    private bool isFalling = false;

    private GroundChecker groundChecker;

    void Start()
    {
        groundChecker = GetComponentInChildren<GroundChecker>();
    }

    private void LateUpdate()
    {
        //
        if (groundChecker.isGrounded && isFalling)
        {
            lastY = firstY - transform.position.y;
            isFalling = false;
        }

        // Controllare Is Grounded
        if (!groundChecker.isGrounded && !isFalling)
        {
            firstY = transform.position.y;
            isFalling = true;
        }

        // Eventuale danno
    }
}