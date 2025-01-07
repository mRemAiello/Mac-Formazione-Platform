using System;
using DG.Tweening;
using GameUtils;
using UnityEngine;

public class FadeToBlack : Singleton<FadeToBlack>
{
    [Header("Settings")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("Fade In")]
    [SerializeField, Range(0.1f, 100f)] private float _fadeInSpeed = 1.0f;
    [SerializeField] private AnimationCurve _fadeInCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Fade Out")]
    [SerializeField, Range(0.1f, 100f)] private float _fadeOutSpeed = 1.0f;
    [SerializeField] private AnimationCurve _fadeOutCurve = AnimationCurve.Linear(0, 1, 1, 0);

    //
    void Start()
    {
        FadeToWhite();
    }

    public void StartFade(Action onFadeComplete = null)
    {
        //
        _canvasGroup.alpha = 0;

        //
        _canvasGroup.DOFade(1, _fadeOutSpeed).SetEase(_fadeOutCurve).onComplete += () =>
        {
            onFadeComplete?.Invoke();
        };
    }

    private void FadeToWhite()
    {
        // 
        _canvasGroup.alpha = 1;

        //
        _canvasGroup.DOFade(0, _fadeInSpeed).SetEase(_fadeInCurve);
    }
}