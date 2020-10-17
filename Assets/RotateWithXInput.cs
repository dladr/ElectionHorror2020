using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RotateWithXInput : MonoBehaviour
{
    [SerializeField] private float maxYRotation;

    [SerializeField] private Vector3 rotationVector3;

    [SerializeField] private Vector3 _currentVector3;

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

        if (_currentVector3.y > 180)
        {
            _currentVector3 = new Vector3(0, _currentVector3.y - 360, 0);
        }

        transform.localEulerAngles =
            Vector3.MoveTowards(_currentVector3, _targetRotation, rotationSpeed * Time.deltaTime);

    }
}


