using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLifeManager : Singleton<PlayerLifeManager>, IDamageable
{
    [Header("References")]
    [SerializeField] private Animator _animator;

    [Header("Dead Animation")]
    [SerializeField] private float _deadAnimationTime;
    [SerializeField] private float _fadeAnimationTime;

    //
    private float _currentHP;
    public float maxHP;
    private float damagePerSecond = 0;
    private float seconds = 0;

    public float hpFillSpeed;
    public float yellowHpFillSpeed;

    //
    public bool IsDead => _currentHP <= 0;
    public bool IsAlive => _currentHP > 0;

    // TODO: Event System
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
        if (IsDead)
            return;

        //
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

        // TODO: Aggiornare UI (Eventi)
    }

    private void Death()
    {
        _animator.SetBool("Dead", true);

        //
        Invoke(nameof(Fade), _deadAnimationTime);
    }

    private void Fade()
    {
        // Dissolvenza a nero
        FadeToBlack.Instance.StartFade();

        //
        Invoke(nameof(Respawn), _fadeAnimationTime);
    }

    private void Respawn()
    {
        //
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}