using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESPoint
{
    public bool spawnable;
    public Vector2 position;

    public ESPoint(bool _spawnable, Vector2 _pos)
    {
        spawnable = _spawnable;
        position = _pos;
    }

    public bool IsVisible()
    {
        Camera cam = Camera.main;
        Vector3 viewPos = cam.WorldToViewportPoint((Vector3)position);
        if((viewPos.x <= 1 && viewPos.x >= 0) && (viewPos.y <= 1 && viewPos.y >= 0)) 
            return true;
        return false;
    }
}
