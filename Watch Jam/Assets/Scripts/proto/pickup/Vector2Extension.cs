using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public static class Vector2Extension
{
    public static Vector2 Rotate( this Vector2 v, float angle )
    {
        float sin = Mathf.Sin( angle * Mathf.Deg2Rad );
        float cos = Mathf.Cos( angle * Mathf.Deg2Rad );

        return new Vector2( cos * v.x - sin * v.y, sin * v.x + cos * v.y );

        // Vector2 dir = v;
        // dir.Normalize();
        //return Quaternion.AngleAxis( angle, -Vector3.forward ) * dir;
    }
}

