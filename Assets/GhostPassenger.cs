using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPassenger : MonoBehaviour
{

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private ParticleSystem _particleSystem;

    [SerializeField] private float rotationSpeed;

    [SerializeField] Vector3 _facePlayerRotation;
    [SerializeField] private Vector3 _faceWindowRotation;
    [SerializeField] private Vector3 _targetRotation;

    [SerializeField] private GameObject _paperPersonGameObject;

    [SerializeField] private bool _isRotatingToPlayer;

   [SerializeField] private float _playerRotationTrigger;
    [SerializeField] private RotateWithXInput _cameraRotateWithXInput;
    [SerializeField] private DialogueTrigger _dialogueTrigger;

    private Vector3 _currentVector3;


    private void Update()
    {
        if (_cameraRotateWithXInput._currentVector3.y < _playerRotationTrigger)
        {
            _dialogueTrigger.ExternalTrigger();
            _isRotatingToPlayer = true;
        }

        LerpTowardsTargetRotation();
    }

    public void Disappear()
    {
        _paperPersonGameObject.SetActive(false);
        _particleSystem.Play();
        _audioSource.Play();
    }

    void LerpTowardsTargetRotation()
    {
        _currentVector3 = MakeEulersUseful(transform.eulerAngles);
        _targetRotation = _isRotatingToPlayer ? _facePlayerRotation : _faceWindowRotation;

        transform.localEulerAngles =
            Vector3.MoveTowards(_currentVector3, _targetRotation, rotationSpeed * Time.deltaTime);

    }

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
