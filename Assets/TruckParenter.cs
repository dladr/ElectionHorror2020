using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Sirenix.Utilities;
using UnityEngine;

public class TruckParenter : MonoBehaviour
{
    public Transform OriginalParent;
    private Transform _truckTransform;
    public bool HasTruck;
    [SerializeField] private AudioSource _audioSource;
    public bool IsReleasingConstraints;

    private void Awake()
    {
        _truckTransform = SingletonManager.Get<TruckMovement>().transform;
        OriginalParent = _truckTransform.parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Truck") && _truckTransform.parent == OriginalParent)
        {
            _truckTransform.parent = transform;
            HasTruck = true;
            _audioSource.Play();

            if(IsReleasingConstraints)
               SingletonManager.Get<TruckMovement>().ReleaseConstraints();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (HasTruck && other.CompareTag("Truck") && !IsReleasingConstraints)
        {
           ReleaseTruck();
        }
    }

    public void ReleaseTruck()
    {
        if (HasTruck)
        {
            _truckTransform.parent = OriginalParent;
            HasTruck = false;
        }
           
    }
}
