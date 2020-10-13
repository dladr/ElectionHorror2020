using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent TheEvent;

    public bool HasTriggered;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Truck") && !HasTriggered)
        {
            HasTriggered = true;
            TheEvent.Invoke();
        }
    }

    public void Reset()
    {
        HasTriggered = false;
    }
}
