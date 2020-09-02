using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform ObjectToFollow;
    
    void LateUpdate()
    {
        transform.position = ObjectToFollow.position;
    }
}
