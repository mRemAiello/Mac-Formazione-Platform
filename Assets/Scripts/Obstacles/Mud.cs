using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : MonoBehaviour
{
    public string playerTag;
    public float slowVelocity = 2;
    public float slowJumpSpeed = 2;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            var playerScript = other.gameObject.GetComponent<PlayerMovement>();
            if (playerScript != null)
            {
                playerScript.Slow(slowVelocity, slowJumpSpeed);
            }
        } 
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            var playerScript = other.gameObject.GetComponent<PlayerMovement>();
            if (playerScript != null)
            {
                playerScript.RemoveSlow();
            }
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            var playerScript = other.gameObject.GetComponent<PlayerMovement>();
            if (playerScript != null)
            {
                playerScript.Slow(slowVelocity, slowJumpSpeed);
            }
        } 
    }
}