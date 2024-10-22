using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    public float currentHP;
    public float maxHP;
    public float damagePerSecond = 0;
    public float seconds = 0;
    public Slider hpSlider;

    void Start()
    {
        currentHP = maxHP;
        hpSlider.maxValue = maxHP;
    }

    void Update()
    {
        if (seconds > 0)
        {
            TakeDamage(damagePerSecond * Time.deltaTime);
            seconds -= Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        hpSlider.value = currentHP;
    }

    public void AddDamagePerSecond(float damage, float seconds)
    {
        damagePerSecond = damage;
        this.seconds = seconds;
    }

    public void RemoveDamagePerSecond()
    {
        damagePerSecond = 0;
        seconds = 0;
    }

    public void TakeDamage(float damage)
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