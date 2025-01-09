using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyVirtualJoystic : Singleton<MyVirtualJoystic>
{
    [Range(0, 1)] public float deadzone = 0.3f;

    private Vector2 axis;

    public float GetAxis(string axe)
    {
        switch (axe.ToLower())
        {
            case "horizontal":
            case "h":
            case "x":
                return axis.x;
            case "vertical":
            case "v":
            case "y":
                return axis.y;
        }
        return 0;
    }


    public float GetAxisRaw(string axe)
    {
        float f = GetAxis(axe);
        if (Mathf.Abs(f) < deadzone || Mathf.Approximately(f, 0))
            return 0;
        return Mathf.Sign(f);
    }
}