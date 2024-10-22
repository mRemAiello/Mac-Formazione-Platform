using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    public float currentHP;
    public float maxHP;
    public float damagePerSecond = 0;
    public float seconds = 0;

    public float hpFillSpeed;
    public float yellowHpFillSpeed;

    public Slider hpSlider;
    public Slider fillHpSlider;

    void Start()
    {
        currentHP = maxHP;

        //
        hpSlider.minValue = 0;
        fillHpSlider.minValue = 0;
        hpSlider.maxValue = maxHP;
        fillHpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
        fillHpSlider.value = currentHP;
    }

    void Update()
    {
        if (seconds > 0)
        {
            TakeDamage(damagePerSecond * Time.deltaTime);
            seconds -= Time.deltaTime;
        }

        //
        float target = Mathf.Lerp(hpSlider.value, currentHP, hpFillSpeed * Time.deltaTime);
        float targetEffect = Mathf.Lerp(fillHpSlider.value, currentHP, yellowHpFillSpeed * Time.deltaTime);

        hpSlider.value = target;
        fillHpSlider.value = targetEffect;
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