using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtendMethod
{
    private const float dotThreshold = 0.5f;

    public static bool isForward(this Transform transform, Transform target)
    {
        Vector3 targetToForward = target.position - transform.position;
        targetToForward.Normalize();
        float dot = Vector3.Dot(transform.forward, targetToForward);
        return dot >= dotThreshold;
    }
}
