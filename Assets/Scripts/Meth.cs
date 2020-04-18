using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meth
{
    public static float pointToDegree(Vector3 targetPosition, Vector3 myPosition)
    {
        Vector3 diff = targetPosition - myPosition;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        return rot_z;
    }

    public static float vector2ToDegree(Vector2 vector2)
    {
        vector2.Normalize();
        return Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg;
    }

    public static float distanceSq(Vector2 a, Vector2 b)
    {
        return (a - b).sqrMagnitude;
    }
}
