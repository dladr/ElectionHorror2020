using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Modifies color to alpha. Use a range between 0 and 1
    /// </summary>
    /// <param name="color">Color to modify</param>
    /// <param name="alpha">Alpha value to set</param>
    public static void SetAlpha(this ref Color color, float alpha)
    {
        if (alpha < 0)
        {
            Debug.LogWarning($"SetAlpha was set to {alpha}. Hoisting to 0");
            alpha = 0;
        }
        else if (alpha > 1)
        {
            Debug.LogWarning($"SetAlpha was set to {alpha}. Lowering to 1");
            alpha = 1;
        }

        color = new Color(color.r, color.g, color.b, alpha);
    }

    /// <summary>
    /// Modifies a Vector 3 by adding all optional new values
    /// </summary>
    /// <param name="v"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public static Vector3 Add(this Vector3 v, float x = 0, float y = 0, float z = 0)
    {
        v = new Vector3(v.x + x, v.y + y, v.z + z);
        return v;
    }

    /// <summary>
    /// Modifies a Vector 3 by writing over any supplied values 
    /// </summary>
    /// <param name="v"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public static Vector3 Modify(this Vector3 v, float? x = null, float? y = null, float? z = null)
    {
        v = new Vector3(x ?? v.x, y ?? v.y, z ?? v.z);
        return v;
    }

    public static T GetRandom<T>(this IEnumerable<T> list)
    {
        var random = Random.Range(0, list.Count());
        return list.ElementAt(random);
    }

    public static float RandomRange(this float f)
    {
        return Random.Range(-f, f);
    }

    public static Vector3 RandomRange(this Vector3 v)
    {
        return new Vector3(
            v.x.RandomRange(),
            v.y.RandomRange(),
            v.z.RandomRange()
        );
    }

    public static void ShuffleArray<T>(this T[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int r = Random.Range(0, i);
            T tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }

    public static string ToLowerRemoveSpaces(this string s)
    {
        return s?.ToLower().Replace(" ", "");
    }
}


