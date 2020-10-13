using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

public class TruckDoor : MonoBehaviour
{
    [SerializeField] private bool _isPlayerPresent;
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private Transform _drivingCameraTransform;
    [SerializeField] private Transform _playerCameraTransform;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _cameraSpeed;
    [SerializeField] private float _threshold;
    [SerializeField] private TruckMovement _truckMovement;
    [SerializeField] private float _cooldownTime;
    [SerializeField] private bool _isCoolingDown;
    [SerializeField] private RearDoor _rearDoor;
    private TextModifier _textModifier;

    private void Awake()
    {
        _textModifier = SingletonManager.Get<TextModifier>();
    }


    void Update()
    {
        if (_isPlayerPresent && Input.GetButtonDown("Action") && !_isCoolingDown)
        {
            if (_rearDoor.IsOpen)
            {
                _textModifier.UpdateTextTrio("I shouldn't drive with the back door open...", Color.white, FontStyles.Normal);
            }

            else
            {
                _isPlayerPresent = false;
                EnterTruck();
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!other.GetComponentInChildren<PlayerController>().IsActive)
                return;

            _isPlayerPresent = true;
            other.GetComponentInChildren<OrbManager>().SetCanAttack(false);
            _textModifier.UpdateTextTrio("Front Door", Color.white, FontStyles.Normal);
            _textModifier.Fade(true, 10f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!other.GetComponentInChildren<PlayerController>().IsActive)
                return;

            _isPlayerPresent = false;
            other.GetComponentInChildren<OrbManager>().SetCanAttack(true);
            _textModifier.Fade(false, 10f);
        }
    }

    void EnterTruck()
    {
        _isCoolingDown = true;
        _textModifier.Fade(false, 10f);

        _playerObject.GetComponent<PlayerController>().ToggleIsActive();
        _mainCamera.transform.SetParent(_drivingCameraTransform);
        _playerObject.transform.SetParent(_drivingCameraTransform);

        //TODO: Cool disappear of character
        _playerObject.GetComponent<PlayerController>().Disappear();

        StartCoroutine(MoveCamera(_drivingCameraTransform));
    }

    public void ExitTruck()
    {
        if (_isCoolingDown)
            return;

        _isCoolingDown = true;

        _truckMovement.ToggleActive();
        _playerObject.transform.SetParent(null);
        _playerObject.GetComponent<PlayerController>().ResetRotation();
        _mainCamera.transform.SetParent(_playerCameraTransform);
        StartCoroutine(MoveCamera(_playerCameraTransform));
    }

    public void ExitTruckInstantly()
    {
        _playerObject.transform.SetParent(null);
        _playerObject.GetComponent<PlayerController>().ResetRotation();
        _mainCamera.transform.SetParent(_playerCameraTransform);
        _mainCamera.transform.position = _playerCameraTransform.position;
        _mainCamera.transform.rotation = _playerCameraTransform.rotation;
        _playerObject.GetComponent<PlayerController>().ReappearWithouActivating();
    }

    void ResetCoolDown()
    {
        _isCoolingDown = false;
    }

    IEnumerator MoveCamera(Transform destinationTransform)
    {
        while (Vector3.Distance(_mainCamera.transform.position, destinationTransform.position) > _threshold || Vector3.Distance(_mainCamera.transform.eulerAngles, destinationTransform.eulerAngles) > _threshold)
        {
            _mainCamera.transform.rotation = Quaternion.RotateTowards(_mainCamera.transform.rotation, destinationTransform.rotation, _cameraSpeed * 20 * Time.deltaTime);
           // _mainCamera.transform.eulerAngles = Vector3.MoveTowards(_mainCamera.transform.eulerAngles, destinationTransform.eulerAngles, _cameraSpeed * 20 * Time.deltaTime);
            _mainCamera.transform.position = Vector3.MoveTowards(_mainCamera.transform.position, destinationTransform.position, _cameraSpeed  * Time.deltaTime);
           
            yield return new  WaitForEndOfFrame();
        }

        if(destinationTransform == _drivingCameraTransform)
            _truckMovement.ToggleActive();
        else
        {
            _playerObject.GetComponent<PlayerController>().Reappear();
        }

        Invoke(nameof(ResetCoolDown), _cooldownTime);

        yield return null;
    }
}
