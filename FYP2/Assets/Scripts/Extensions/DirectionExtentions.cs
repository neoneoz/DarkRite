
using UnityEngine;
using System.Collections;
 
public static class DirectionsExtensions
{
    public static Directions GetDirection (this BGrid t1, BGrid t2)
    {
        if (t1.index.y < t2.index.y)
            return Directions.North;
        if (t1.index.x < t2.index.x)
            return Directions.East;
        if (t1.index.y > t2.index.y)
            return Directions.South;
        return Directions.West;
    }
 
    public static Vector3 ToEuler (this Directions d)
    {
        return new Vector3(0, (int)d * 90, 0);
    }
}