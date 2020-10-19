using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
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

    [SerializeField] public float CurrentSpeed;

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

    private float _storedSpeed;
    private Vector3 _storedVelocity;

    private TruckDoor _truckDoor;

    public bool IsCopNearby;

    [SerializeField] private Camera CenterMirrorCamera;
    [SerializeField] private Camera LeftMirrorCamera;

    [SerializeField]private RotateWithXInput[] _rotateWithXInputs;
    [SerializeField] private AudioSource _engineAudioSource;

    [SerializeField] private bool _isInDangerZone;

    [SerializeField] private DangerZoneTrigger _dangerZoneTrigger;
 
    // Start is called before the first frame update
    void Awake()
    {
        _truckDoor = SingletonManager.Get<TruckDoor>();
        _engineAudioSource = GetComponent<AudioSource>();
        _rotateWithXInputs = GetComponentsInChildren<RotateWithXInput>();
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

        if(!IsHidden)
            CalculateManualSpeed(Input.GetAxis("Vertical"));

        //Accelerate(Input.GetAxis("Vertical"));

        //_currentSpeed = transform.InverseTransformDirection(_rigidbody.velocity).z;
        _velocity = _rigidbody.velocity;

        if (!IsHidden && CurrentSpeed < .5f && Input.GetButtonDown("Action"))
        {
            _truckDoor.ExitTruck();
        }

        if (_isInDangerZone && CurrentSpeed < _dangerZoneTrigger.MinSpeed)
        {
            CaughtByCops();
        }

    }

    public void CaughtByCops()
    {
        Deactivate();
       _truckDoor.ExitTruckInstantly();
       SingletonManager.Get<GameManager>().LastCheckPoint.Reset();
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

        _storedSpeed = transform.InverseTransformDirection(_rigidbody.velocity).z;
        _storedVelocity = _rigidbody.velocity;

        CurrentSpeed = 0;
        _rigidbody.velocity = Vector3.zero;

        yield return new WaitForSeconds(TimeToHide);

        while (IsCopNearby)
        {
            yield return new WaitForEndOfFrame();
        }

        IsHidden = false;
        SteeringWheelSpriteRenderer.color = Color.white;

        //_currentSpeed = _storedSpeed;
        //_rigidbody.velocity = _storedVelocity;

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
        _rigidbody.transform.Rotate(0, xAxis * _turnForce * Time.deltaTime * CurrentSpeed, 0);
    }

    void Accelerate(float yAxis)
    {
        if (CurrentSpeed >= _maxSpeed && yAxis > 0 || CurrentSpeed <= _minSpeed && yAxis < 0)
            return;

        _rigidbody.AddForce(transform.forward * (yAxis * _accelerationForce), ForceMode.Force);
    }

    void CalculateManualSpeed(float yAxis)
    {
        if (IsHidden)
        {
            CurrentSpeed = 0;
            _rigidbody.velocity = Vector3.zero;
            return;
        }

        CurrentSpeed = transform.InverseTransformDirection(_rigidbody.velocity).z;
        float speedMagnitude = Mathf.Abs(CurrentSpeed);
        if (speedMagnitude < .01f)
            CurrentSpeed = speedMagnitude = 0;
        float speedPercent = speedMagnitude / _maxSpeed;
        if (CurrentSpeed > 0)
        {
            if (yAxis > 0)
            {
                CurrentSpeed += Time.deltaTime * yAxis * (_minimumAcceleration + _accelerationForce * speedPercent);
            }

            else
            {
                CurrentSpeed += Time.deltaTime * yAxis * _breakForce * speedPercent;
            }
        }
          
        else if (CurrentSpeed < 0)
        {
            if (yAxis <= 0)
            {
                CurrentSpeed += Time.deltaTime * yAxis * (_minimumAcceleration + _accelerationForce * speedPercent);
            }

            else
            {
                CurrentSpeed += Time.deltaTime * yAxis * _breakForce * speedPercent;
            }
            
        }

        else
        {
            CurrentSpeed += Time.deltaTime * yAxis * (_minimumAcceleration + _accelerationForce * speedPercent);
        }
        CurrentSpeed -= CalculateDrag();

        if (CurrentSpeed > _maxSpeed)
            CurrentSpeed = _maxSpeed;
        if (CurrentSpeed < _minSpeed)
            CurrentSpeed = _minSpeed;

        _rigidbody.velocity = transform.forward * CurrentSpeed;

        float percentSpeed = Mathf.Abs(CurrentSpeed) / _maxSpeed;
        _engineAudioSource.pitch = 1 + percentSpeed * 2;
    }

    float CalculateDrag()
    {
        if (CurrentSpeed == 0)
            return 0f;

        float multiplier = 1;
        if (CurrentSpeed < 0)
            multiplier = -1;

        float friction = (_frictionFactor*_minimumAcceleration * multiplier * Time.deltaTime) + (_frictionFactor * CurrentSpeed * Time.deltaTime);
        if (Mathf.Abs(friction) > Mathf.Abs(CurrentSpeed))
        {
            friction = CurrentSpeed;
        }

        return friction;
    }

    public void ToggleActive()
    {
        _isActive = !_isActive;
        _rigidbody.isKinematic = !_isActive;
        _capsuleColliderGameObject.SetActive(_isActive);
        _boxColliderGameObject.SetActive(!_isActive);

        CenterMirrorCamera.enabled = _isActive;
        LeftMirrorCamera.enabled = _isActive;

        foreach (RotateWithXInput rotateWithXInput in _rotateWithXInputs)
        {
            rotateWithXInput.IsRotating = _isActive;
        }
    }

    public void Deactivate()
    {
        _isActive = false;
        _rigidbody.isKinematic = !_isActive;
        _capsuleColliderGameObject.SetActive(_isActive);
        _boxColliderGameObject.SetActive(!_isActive);

        CenterMirrorCamera.enabled = _isActive;
        LeftMirrorCamera.enabled = _isActive;

        foreach (RotateWithXInput rotateWithXInput in _rotateWithXInputs)
        {
            rotateWithXInput.IsRotating = _isActive;
        }
    }

    public void EnterDangerZone(DangerZoneTrigger dangerZoneTrigger)
    {
        _isInDangerZone = true;
        _dangerZoneTrigger = dangerZoneTrigger;
    }

    public void ExitDangerZone()
    {
        _isInDangerZone = false;
        _dangerZoneTrigger = null;
    }
}
