using System;
using Assets.Scripts.Helpers;
using DefaultNamespace.Attributes;
using UnityEngine;

[RequireComponent(typeof(MonoBehaviourExtra))]
public class MonoBehaviorExtraProxy : MonoBehaviour
{
    private static MonoBehaviorExtraProxy _helper;

    [AutoAssign]
    protected MonoBehaviourExtra _behaviourExtra;

    public static MonoBehaviorExtraProxy Get
    {
        get
        {
            if (_helper == null)
            {
                _helper = SingletonManager.Get<MonoBehaviorExtraProxy>();
            }

            return _helper;
        }
    }

    public static void Invoke(Action action, float timer)
    {
        Get._behaviourExtra.StartCoroutine(action, timer);
    }
}
