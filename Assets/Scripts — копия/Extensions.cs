using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T Random<T>(this T[] array)
    {
        return array.Length == 0? default: array[UnityEngine.Random.Range(0, array.Length)];
    }

    public static T Random<T>(this List<T> list)
    {
        return list.Count == 0? default: list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static void ZPositionTo(this Transform transform, float newZ)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }
}
