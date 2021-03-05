using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBagProtector : MonoBehaviour
{
    public float MinimumHeight;

    public float ResetHeight;

   
    void Update()
    {
        if(transform.position.y < MinimumHeight)
            transform.position = new Vector3(transform.position.x, ResetHeight, transform.position.z);
    }
}
