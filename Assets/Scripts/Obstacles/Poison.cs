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
            PlayerLifeManager.Instance.RemoveDamagePerSecond();
            PlayerController.Instance.Slow(slowVelocity, slowJumpSpeed);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            PlayerController.Instance.RemoveSlow();
            PlayerLifeManager.Instance.AddDamagePerSecond(damagePerSecond, seconds);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            PlayerController.Instance.Slow(slowVelocity, slowJumpSpeed);
            PlayerLifeManager.Instance.RemoveDamagePerSecond();
            PlayerLifeManager.Instance.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}