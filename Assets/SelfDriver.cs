using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Scripts.Helpers;
using UnityEngine;

public class SelfDriver : MonoBehaviour
{
    [SerializeField] private Transform _startingPointTransform;

    [SerializeField] private Transform _endingPointTransform;

    [SerializeField] private Transform _currentTargetTransform;

    [SerializeField] private bool _hasHitStartingPoint;

    [SerializeField] private bool _isActive;

    [SerializeField] private TruckMovement _truckMovement;

    

    // Start is called before the first frame update
    void Awake()
    {
        _truckMovement = SingletonManager.Get<TruckMovement>();
        _currentTargetTransform = _startingPointTransform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive)
            return;

        _truckMovement.transform.LookAt(_currentTargetTransform);
        _truckMovement.CalculateManualSpeed(1);

    }

    public void SwitchTargets()
    {
        _currentTargetTransform = _endingPointTransform;
    }

    public void StartSelfDriving()
    {
        _truckMovement.StartSelfDriving();
        _isActive = true;
    }

    public void EndSelfDriving()
    {
        _truckMovement.EndSelfDriving();
        _isActive = false;
    }

    //void SmoothRotation()
    //{
        

    //    Vector3 _currentVector3 = MakeEulersUseful(transform.eulerAngles);

    //    Vector3 _targetVector3 = MakeEulersUseful(_currentRotation.eulerAngles);


    //    transform.localEulerAngles =
    //        Vector3.MoveTowards(_currentVector3, _targetVector3, _rotationSpeed * Time.deltaTime);

    //    Debug.Log(_targetVector3);
    //}

    Vector3 MakeEulersUseful(Vector3 _currentVector3)
    {
        float _currentY = _currentVector3.y;
        float _currentZ = _currentVector3.z;
        float _currentX = _currentVector3.x;

        if (_currentY > 180)
        {
            _currentY -= 360;
        }

        if (_currentZ > 180)
        {
            _currentZ -= 360;
        }

        if (_currentX > 180)
        {
            _currentX -= 360;
        }

        return new Vector3(_currentX, _currentY, _currentZ);
    }
}
