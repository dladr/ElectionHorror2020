using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using UnityEngine;

public class RotationReference : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SingletonManager.Get<GameManager>().AddRotationReference(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
