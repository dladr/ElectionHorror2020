using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BulletHoleManager : MonoBehaviour
{
    private BulletHoleAdder[] _bulletHoleAdders;
    // Start is called before the first frame update
    void Start()
    {
        _bulletHoleAdders = GetComponentsInChildren<BulletHoleAdder>();
    }

    public void AddBulletHoles()
    {
        foreach (BulletHoleAdder bulletHoleAdder in _bulletHoleAdders)
        {
            bulletHoleAdder.AddBulletHoles();
        }
    }

    [Button]
    public void Reset()
    {
        foreach (BulletHoleAdder bulletHoleAdder in _bulletHoleAdders)
        {
            bulletHoleAdder.Reset();
        }
    }
}
