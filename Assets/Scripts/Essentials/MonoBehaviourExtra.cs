using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class MonoBehaviourExtra : MonoBehaviour
{
    public void StartCoroutine(Action action, float timer)
    {
        StartCoroutine(InvokeCo(action, timer));
    }

    IEnumerator InvokeCo(Action a, float timer)
    {
        yield return new WaitForSeconds(timer);
        a();
    }
}


