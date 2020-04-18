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
}
