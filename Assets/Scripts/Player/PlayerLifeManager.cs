using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeManager : Singleton<PlayerLifeManager>
{
    private float _currentHP;
    public float maxHP;
    private float damagePerSecond = 0;
    private float seconds = 0;

    public float hpFillSpeed;
    public float yellowHpFillSpeed;

    public Slider hpSlider;
    public Slider fillHpSlider;

    void Start()
    {
        _currentHP = maxHP;

        //
        hpSlider.minValue = 0;
        fillHpSlider.minValue = 0;
        hpSlider.maxValue = maxHP;
        fillHpSlider.maxValue = maxHP;
        hpSlider.value = _currentHP;
        fillHpSlider.value = _currentHP;
    }

    void Update()
    {
        if (seconds > 0)
        {
            TakeDamage(damagePerSecond * Time.deltaTime);
            seconds -= Time.deltaTime;
        }

        //
        float target = Mathf.Lerp(hpSlider.value, _currentHP, hpFillSpeed * Time.deltaTime);
        float targetEffect = Mathf.Lerp(fillHpSlider.value, _currentHP, yellowHpFillSpeed * Time.deltaTime);

        hpSlider.value = target;
        fillHpSlider.value = targetEffect;
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
        _currentHP -= damage;
        if (_currentHP <= 0)
        {
            _currentHP = 0;
            Death();
        }

        // TODO: Aggiornare UI
    }

    private void Death()
    {
        // TODO: Animazione
    }
}