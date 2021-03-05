using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using UnityEngine;

public class LaneBraker : MonoBehaviour
{

    [SerializeField] public bool IsActive;
    [SerializeField] private Transform _truckTransform;
    [SerializeField] private Transform _aheadTransform;
    [SerializeField] private Transform _behindTransform;
    [SerializeField] private Transform _finalPosition;
    public float distance1;
    public float distance2;
    public float Speed;
    public int Direction = 1;

   [SerializeField] private Vector3 _startingPosition;

   public bool HasReachedFinalPosition;

    

    void Awake()
    {
        _truckTransform = SingletonManager.Get<TruckMovement>().transform;
        _startingPosition = transform.localPosition;
    }

    void Update()
    {
        if (!IsActive)
            return; 

        SetDirection(); 
        Move();
    }

    void SetDirection()
    {
         distance1 = Vector3.Distance(_truckTransform.position, _aheadTransform.position);
         distance2 = Vector3.Distance(_truckTransform.position, _behindTransform.position);

         Direction = distance1 > distance2 ? -1 : 1;

    }

    void Move()
    {
        if (transform.localPosition.z > _finalPosition.localPosition.z)
        {
            IsActive = false;
            transform.localPosition = _finalPosition.localPosition;
            HasReachedFinalPosition = true;
            return;
        }

        transform.position += transform.forward * Direction * Speed * Time.deltaTime;
    }

    public void Reset()
    {
        IsActive = false;
        transform.localPosition = _startingPosition;
        HasReachedFinalPosition = false;
    }
}
