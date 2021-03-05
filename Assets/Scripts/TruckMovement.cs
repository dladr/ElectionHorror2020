using System;
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
    [SerializeField] private GameObject _ghostPassengerParentGameObject;

    public bool IsDrivingWithDoorOpen;
    public bool IsHidden;
    public bool IsTryingToHide;
    public bool CanHide;
    public bool CanHideExternal;
    public float HideTime;
    public float HideCooldown;
    public float TimeToHide;
    public bool IsButtonDown;
    public SpriteRenderer SteeringWheelSpriteRenderer;
    public Color HideChargedColor;
    public Color HideColor;

    private bool _isCruiseControl;
    private Vector3 _cruiseControlVelocity;


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

    [SerializeField] private RigidbodyConstraints _initialRigidbodyConstraints;
                     private RigidbodyConstraints _releasedConstraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
      
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private AudioSource _particleAudioSource;

    [SerializeField] private GameObject _strangerOrbGameObject;

    public bool IsAutoDriving;

    private float _horizontalInput;
    private float _verticalInput;

    
    public SpriteRenderer CruiseControlRenderer;
    public SpriteRenderer CButtonRederer;
    public Color CActiveColor;
    public Color CDeactiveColor;
    
 
    // Start is called before the first frame update
    void Awake()
    {
        _truckDoor = SingletonManager.Get<TruckDoor>();
        _engineAudioSource = GetComponent<AudioSource>();
        _rotateWithXInputs = GetComponentsInChildren<RotateWithXInput>();
        _initialRigidbodyConstraints = _rigidbody.constraints;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive)
            return;

        if(Input.GetKeyDown(KeyCode.C))
            ToggleCruiseControl();


        IsButtonDown = Input.GetButton("Action");
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        if (CanHideExternal && CanHide && Input.GetButtonDown("Action"))
            StartCoroutine(Hide());

      //  Turn(Input.GetAxis("Horizontal"));

        //if(!IsHidden)
        //    CalculateManualSpeed(Input.GetAxis("Vertical"));

       
     //   _velocity = _rigidbody.velocity;

        if (!IsHidden && CurrentSpeed < .5f && Input.GetButtonDown("Action"))
        {
            _truckDoor.ExitTruck();
        }

        if (_isInDangerZone && CurrentSpeed < _dangerZoneTrigger.MinSpeed)
        {
            CaughtByCops();
        }

    }

    private void FixedUpdate()
    {
        if (!_isActive)
            return;

        Turn(Input.GetAxis("Horizontal"));

        if (!IsHidden)
            CalculateManualSpeed(Input.GetAxis("Vertical"));

        _velocity = _rigidbody.velocity;

    }

    private void LateUpdate()
    {
        //if (!_isActive)
        //{
        //    CurrentSpeed = transform.InverseTransformDirection(_rigidbody.velocity).z;
        //}

        if(IsAutoDriving)
            transform.localEulerAngles = Vector3.up * transform.localEulerAngles.y;

        float percentSpeed = Mathf.Abs(CurrentSpeed) / _maxSpeed;
        _engineAudioSource.pitch = 1 + percentSpeed * 2;
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

        EndCruiseControl();
        _particleSystem.Play();
        _particleAudioSource.Play();

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
        _particleSystem.Stop();
        _particleAudioSource.Stop();

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

    IEnumerator HideExternal()
    {
        EndCruiseControl();

        _particleSystem.Play();
        _particleAudioSource.Play();

        float timePassed = 0;
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
        _particleSystem.Stop();
        _particleAudioSource.Stop();
        SteeringWheelSpriteRenderer.color = Color.white;


        while (timePassed < HideCooldown)
        {
            timePassed += Time.deltaTime;
            SteeringWheelSpriteRenderer.color = Color.Lerp(Color.white, HideChargedColor, timePassed / HideCooldown);
            yield return new WaitForEndOfFrame();
        }

        CanHide = true;
    }
  public  void Turn(float xAxis)
  {
      if (_isCruiseControl)
          return;

        //_rigidbody.AddTorque(0, xAxis * _turnForce, 0, ForceMode.Force);
        _rigidbody.transform.Rotate(0, xAxis * _turnForce * Time.deltaTime * CurrentSpeed, 0);
    }

    void Accelerate(float yAxis)
    {
        if (CurrentSpeed >= _maxSpeed && yAxis > 0 || CurrentSpeed <= _minSpeed && yAxis < 0)
            return;

        _rigidbody.AddForce(transform.forward * (yAxis * _accelerationForce), ForceMode.Force);
    }

   public void CalculateManualSpeed(float yAxis)
    {
        if (IsHidden)
        {
            CurrentSpeed = 0;
            _rigidbody.velocity = Vector3.zero;
            return;
        }

        if (IsDrivingWithDoorOpen)
            yAxis = 1;

        if (_isCruiseControl)
        {
            CurrentSpeed = transform.InverseTransformDirection(_rigidbody.velocity).z;
            if(CurrentSpeed > .1f)
             _rigidbody.velocity = _cruiseControlVelocity;
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
        _ghostPassengerParentGameObject.SetActive(_isActive);

        CenterMirrorCamera.enabled = _isActive;
        LeftMirrorCamera.enabled = _isActive;

        foreach (RotateWithXInput rotateWithXInput in _rotateWithXInputs)
        {
            rotateWithXInput.IsRotating = _isActive;
        }

        if(!_isActive)
            EndCruiseControl();
    }

    public void Deactivate()
    {
        _isActive = false;
        _rigidbody.isKinematic = !_isActive;
        _capsuleColliderGameObject.SetActive(_isActive);
        _boxColliderGameObject.SetActive(!_isActive);
        _ghostPassengerParentGameObject.SetActive(_isActive);

        CenterMirrorCamera.enabled = _isActive;
        LeftMirrorCamera.enabled = _isActive;

        EndCruiseControl();

        foreach (RotateWithXInput rotateWithXInput in _rotateWithXInputs)
        {
            rotateWithXInput.IsRotating = _isActive;
        }

        CurrentSpeed = 0;
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

    public void SetCanHideExternal(bool canHideExternal)
    {
        CanHideExternal = canHideExternal;
    }

    public void ExternalHide()
    {
        StartCoroutine(HideExternal());
    }

    public void StartSelfDriving()
    {
        EndCruiseControl();

        _rotateWithXInputs[1].IsRotating = false;
        _isActive = false;
        IsAutoDriving = true;
    }

    public void EndSelfDriving()
    {
        _rotateWithXInputs[1].IsRotating = true;
        _isActive = true;
        IsAutoDriving = false;
    }

    void StartCruiseControl()
    {
        if (IsDrivingWithDoorOpen)
            return;

        _cruiseControlVelocity = _rigidbody.velocity;
        _isCruiseControl = true;
        _rotateWithXInputs[1].IsRotating = false;
        CButtonRederer.color = CruiseControlRenderer.color = CActiveColor;
    }

    void EndCruiseControl()
    {
        _isCruiseControl = false;
        _rotateWithXInputs[1].IsRotating = true;
        CButtonRederer.color = CruiseControlRenderer.color = CDeactiveColor;
    }

    void ToggleCruiseControl()
    {
        if(_isCruiseControl)
            EndCruiseControl();
        else
        {
            StartCruiseControl();
        }
    }

    public void ResetConstraints()
    {
        _rigidbody.constraints = _initialRigidbodyConstraints;
    }

    public void ReleaseConstraints()
    {
        _rigidbody.constraints = _releasedConstraints;
    }

    public void ReleaseALLConstraints()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
    }

    public void FinalDeactivate()
    {
        _engineAudioSource.loop = false;
        _engineAudioSource.playOnAwake = false;
        _engineAudioSource.volume = 0;
        _engineAudioSource.Stop();
        Deactivate();
    }
    
}
