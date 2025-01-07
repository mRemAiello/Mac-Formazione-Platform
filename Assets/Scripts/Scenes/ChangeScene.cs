using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private string _playerTag;

    //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_playerTag))
        {
            FadeToBlack.Instance.StartFade(OnFadeComplete);  
        }
    }

    private void OnFadeComplete()
    {
        SceneManager.LoadScene(_sceneName);
    }
}