using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public float TimeLeft = 5f;

    private void Update()
    {
        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= 0)
        {
            LightHandler.Instance.StartEffect(Color.green, 5f);
            TimeLeft = 0;
        }
    }
}