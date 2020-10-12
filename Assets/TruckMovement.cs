using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckMovement : MonoBehaviour
{
    [SerializeField] private float _accelerationForce;
    [SerializeField] private float _minimumAcceleration;
    [SerializeField] private float _frictionFactor;
    [SerializeField] private float _breakForce;

    [SerializeField] private float _maxSpeed;

    [SerializeField] private float _minSpeed;

    [SerializeField] private float _turnForce;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Vector3 _velocity;

    [SerializeField] private float _currentSpeed;

    [SerializeField] private bool _isActive;
    [SerializeField] private GameObject _boxColliderGameObject;
    [SerializeField] private GameObject _capsuleColliderGameObject;

    public bool IsHidden;
    public bool IsTryingToHide;
    public bool CanHide;
    public float HideTime;
    public float HideCooldown;
    public float TimeToHide;
    public bool IsButtonDown;
    public SpriteRenderer SteeringWheelSpriteRenderer;
    public Color HideChargedColor;
    public Color HideColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive)
            return;

        IsButtonDown = Input.GetButton("Action");

        if (CanHide && Input.GetButtonDown("Action"))
            StartCoroutine(Hide());

        Turn(Input.GetAxis("Horizontal"));
        CalculateManualSpeed(Input.GetAxis("Vertical"));

        //Accelerate(Input.GetAxis("Vertical"));

        //_currentSpeed = transform.InverseTransformDirection(_rigidbody.velocity).z;
        _velocity = _rigidbody.velocity;

        if (_currentSpeed < .5f && Input.GetButtonDown("Action"))
        {
            FindObjectOfType<TruckDoor>().ExitTruck();
        }

    }

    IEnumerator Hide()
    {
        float timePassed = 0;
        IsTryingToHide = true;

        while (IsButtonDown && timePassed < TimeToHide)
        {
            timePassed += Time.deltaTime;
            SteeringWheelSpriteRenderer.color = Color.Lerp(HideChargedColor, HideColor, timePassed / TimeToHide);
            yield return new WaitForEndOfFrame();
        }

        IsTryingToHide = false;

        if (!IsButtonDown)
            yield break;
        
            

        timePassed = 0;
        IsHidden = true;
        CanHide = false;

        yield return new WaitForSeconds(TimeToHide);

        IsHidden = false;
        SteeringWheelSpriteRenderer.color = Color.white;

        while (timePassed < HideCooldown)
        {
            timePassed += Time.deltaTime;
            SteeringWheelSpriteRenderer.color = Color.Lerp(Color.white, HideChargedColor, timePassed / HideCooldown);
            yield return new WaitForEndOfFrame();
        }

        CanHide = true;
        //TODO: Cool hiding effect (maybe turn all renderers off/invisible?)

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

        _rigidbody.AddForce(transform.forward * (yAxis * _accelerationForce), ForceMode.Force);
    }

    void CalculateManualSpeed(float yAxis)
    {
        _currentSpeed = transform.InverseTransformDirection(_rigidbody.velocity).z;
        float speedMagnitude = Mathf.Abs(_currentSpeed);
        if (speedMagnitude < .01f)
            _currentSpeed = speedMagnitude = 0;
        float speedPercent = speedMagnitude / _maxSpeed;
        if (_currentSpeed > 0)
        {
            if (yAxis > 0)
            {
                _currentSpeed += Time.deltaTime * yAxis * (_minimumAcceleration + _accelerationForce * speedPercent);
            }

            else
            {
                _currentSpeed += Time.deltaTime * yAxis * _breakForce * speedPercent;
            }
        }
          
        else if (_currentSpeed < 0)
        {
            if (yAxis <= 0)
            {
                _currentSpeed += Time.deltaTime * yAxis * (_minimumAcceleration + _accelerationForce * speedPercent);
            }

            else
            {
                _currentSpeed += Time.deltaTime * yAxis * _breakForce * speedPercent;
            }
            
        }

        else
        {
            _currentSpeed += Time.deltaTime * yAxis * (_minimumAcceleration + _accelerationForce * speedPercent);
        }
        _currentSpeed -= CalculateDrag();

        if (_currentSpeed > _maxSpeed)
            _currentSpeed = _maxSpeed;
        if (_currentSpeed < _minSpeed)
            _currentSpeed = _minSpeed;

        _rigidbody.velocity = transform.forward * _currentSpeed;
    }

    float CalculateDrag()
    {
        if (_currentSpeed == 0)
            return 0f;

        float multiplier = 1;
        if (_currentSpeed < 0)
            multiplier = -1;

        float friction = (_frictionFactor*_minimumAcceleration * multiplier * Time.deltaTime) + (_frictionFactor * _currentSpeed * Time.deltaTime);
        if (Mathf.Abs(friction) > Mathf.Abs(_currentSpeed))
        {
            friction = _currentSpeed;
        }

        return friction;
    }

    public void ToggleActive()
    {
        _isActive = !_isActive;
        _rigidbody.isKinematic = !_isActive;
        _capsuleColliderGameObject.SetActive(_isActive);
        _boxColliderGameObject.SetActive(!_isActive);
    }
}
