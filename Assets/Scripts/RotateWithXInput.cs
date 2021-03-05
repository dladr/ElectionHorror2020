using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RotateWithXInput : MonoBehaviour
{
    [SerializeField] private float maxYRotation;

    [SerializeField] private Vector3 rotationVector3;

    [SerializeField] public Vector3 _currentVector3;

    [SerializeField] private float rotationSpeed;

    public bool IsRotating;

  [SerializeField]  private Vector3 _targetRotation;

  
   
    void Update()
    {
        if (!IsRotating)
            return;

        SetTargetRotation(Input.GetAxis("Horizontal"));
        LerpTowardsTargetRotation();
    }

    void SetTargetRotation(float xInput)
    {
        
        _targetRotation = rotationVector3 * ((xInput * maxYRotation));

    }

    void LerpTowardsTargetRotation()
    {
        _currentVector3 = transform.localEulerAngles;
        float _currentY = _currentVector3.y;
        float _currentZ = _currentVector3.z;

        if (_currentY > 180)
        {
            _currentY -= 360;
        }

        if (_currentZ > 180)
        {
            _currentZ -= 360;
        }

        _currentVector3 = new Vector3(0, _currentY, _currentZ);

        transform.localEulerAngles =
            Vector3.MoveTowards(_currentVector3, _targetRotation, rotationSpeed * Time.deltaTime);

    }
}


