﻿using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using UnityEngine;
using UnityEngine.Events;

public class SetTruckRestrictions : MonoBehaviour
{
    public UnityEvent TriggerEvent;
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
        if (other.CompareTag("Truck"))
        {
          //  Debug.Log("Setting truck restrictions");
            SingletonManager.Get<RearDoor>().IsMailToCollectNearby = true;
            TriggerEvent.Invoke();
        }
           
    }
}
