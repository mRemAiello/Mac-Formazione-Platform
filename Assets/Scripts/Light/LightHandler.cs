using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightHandler : Singleton<LightHandler>
{
    [SerializeField] private Light2D _light;
    [SerializeField] private float _duration;

    //
    private Color _currentColor;
    private Color _startColor;
    private Color _endColor;
    private bool _isEffectOn = false;

    void Update()
    {
        if (_isEffectOn)
        {
            _light.color = Color.Lerp(_light.color, _endColor, Time.deltaTime * _duration);
            if (_light.color == _endColor)
            {
                _isEffectOn = false;
            }
        }
    }

    public void StartEffect(Color endColor)
    {
        _startColor = _light.color;
        _endColor = endColor;
        _isEffectOn = true;   
    }
}
