using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DangerZoneTrigger : MonoBehaviour
{
    public UnityEvent OnEnterEvent;
    public UnityEvent OnExitEvent;
    public UnityEvent OnCaughtEvent;

    public bool HasTriggered;

    public float MinSpeed;
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Truck"))
        {
            OnEnterEvent.Invoke();
        }

        if (other.CompareTag("Player"))
        {
            SingletonManager.Get<PlayerController>().TakeDamage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Truck"))
        {
            OnExitEvent.Invoke();
            
        }
    }

    public void Reset()
    {
        OnExitEvent.Invoke();
    }

    public void TriggerOnCaught()
    {
        OnCaughtEvent.Invoke();;
    }
}
