using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Slider hpSlider;
    public Slider fillHpSlider;
    public float hpFillSpeed;
    public float yellowHpFillSpeed;

    void Start()
    {
        //
        Invoke(nameof(StartingValues), 0.1f);
    }

    void StartingValues()
    {
        hpSlider.minValue = 0;
        fillHpSlider.minValue = 0;
        hpSlider.maxValue = PlayerController.Instance.maxHP;
        fillHpSlider.maxValue = PlayerController.Instance.maxHP;
        hpSlider.value = PlayerController.Instance.CurrentHP;
        fillHpSlider.value = PlayerController.Instance.CurrentHP;
    }

    void Update()
    {
        if (!PlayerController.InstanceExists)
            return;

        //
        float currentHP = PlayerController.Instance.CurrentHP;

        //
        float target = Mathf.Lerp(hpSlider.value, currentHP, hpFillSpeed * Time.deltaTime);
        float targetEffect = Mathf.Lerp(fillHpSlider.value, currentHP, yellowHpFillSpeed * Time.deltaTime);

        //
        hpSlider.value = target;
        fillHpSlider.value = targetEffect;  
    }
}