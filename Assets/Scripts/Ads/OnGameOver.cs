using System.Collections;
using System.Collections.Generic;
using GamePix;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnGameOver : MonoBehaviour
{
    void Start()
    {
        // show interstital ads
        Gpx.Ads.InterstitialAd(OnInterstitalAdSuccess);
    }

    [AOT.MonoPInvokeCallback(typeof(Gpx.gpxCallback))]
    public static void OnInterstitalAdSuccess()
    {
        //SceneManager.LoadScene("GameOver");
        
    }
}