using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckMovement : MonoBehaviour
{
    [SerializeField] private float _accelerationForce;

    [SerializeField] private float _maxSpeed;

    [SerializeField] private float _minSpeed;

    [SerializeField] private float _turnForce;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Vector3 _velocity;

    [SerializeField] private float _currentSpeed;

    [SerializeField] private bool _isActive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive)
            return;

        Turn(Input.GetAxis("Horizontal"));
        Accelerate(Input.GetAxis("Vertical"));

        _currentSpeed = transform.InverseTransformDirection(_rigidbody.velocity).z;
        _velocity = _rigidbody.velocity;

        if (_currentSpeed < .5f && Input.GetButtonDown("Action"))
        {
            ToggleActive();
            FindObjectOfType<TruckDoor>().ExitTruck();
        }

    }

    void Turn(float xAxis)
    {
        //_rigidbody.AddTorque(0, xAxis * _turnForce, 0, ForceMode.Force);
        _rigidbody.transform.Rotate(0, xAxis * _turnForce * Time.deltaTime * _currentSpeed, 0);
    }

    void Accelerate(float yAxis)
    {
        if (_currentSpeed >= _maxSpeed && yAxis > 0 || _currentSpeed <= _minSpeed && yAxis < 0)
            return;

        _rigidbody.AddForce(transform.forward * (yAxis * _accelerationForce), ForceMode.Acceleration);
    }

    public void ToggleActive()
    {
        _isActive = !_isActive;
    }
}
