using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public string playerTag;
    public int damagePerSecond;
    public int seconds;
    public float slowVelocity = 2;
    public float slowJumpSpeed = 2;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            var playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            var playerLife = other.gameObject.GetComponent<PlayerLife>();
            if (playerMovement != null && playerLife != null)
            {
                playerLife.RemoveDamagePerSecond();
                playerMovement.Slow(slowVelocity, slowJumpSpeed);
            }
        } 
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            var playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            var playerLife = other.gameObject.GetComponent<PlayerLife>();
            if (playerMovement != null && playerLife != null)
            {
                playerMovement.RemoveSlow();
                playerLife.AddDamagePerSecond(damagePerSecond, seconds);
            }
        } 
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            var playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            var playerLife = other.gameObject.GetComponent<PlayerLife>();
            if (playerMovement != null && playerLife != null)
            {
                playerMovement.Slow(slowVelocity, slowJumpSpeed);
                playerLife.RemoveDamagePerSecond();
                playerLife.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}
