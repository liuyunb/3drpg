using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    private static float dotArea = 0.5f;

    public static bool IsAttackArea(this Transform transform,Transform target)
    {
        Vector3 dir = target.position - transform.position;
        float dot = Vector3.Dot(dir, transform.forward);

        return dot >= dotArea;
    }
}
