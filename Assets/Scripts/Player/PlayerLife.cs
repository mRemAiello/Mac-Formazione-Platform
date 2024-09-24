using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public int currentHP;
    public int maxHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            Death();
        }

        // TODO: Aggiornare UI
    }

    private void Death()
    {
        // TODO: Animazione
    }
}