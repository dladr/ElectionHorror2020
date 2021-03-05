using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGrandmaDead : MonoBehaviour
{
    
    void Start()
    {
        GetComponent<Animator>().Play("GrandmaDead");
    }

   
}
