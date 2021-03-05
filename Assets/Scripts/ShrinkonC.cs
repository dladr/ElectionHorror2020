using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkonC : MonoBehaviour
{
    public float ShrunkScale;
    public float DefaultScale;


    private void Awake()
    {
        DefaultScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            transform.localScale = Vector3.one * ShrunkScale;
        }

        else
        {
            transform.localScale = Vector3.one * DefaultScale;
        }
    }
}
