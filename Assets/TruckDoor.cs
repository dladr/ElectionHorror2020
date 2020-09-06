using System;
using System.Collections;
using System.Collections.Generic;
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

    
    void Update()
    {
        if (_isPlayerPresent && Input.GetButtonDown("Action"))
        {
            _isPlayerPresent = false;
            EnterTruck();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerPresent = true;
            other.GetComponentInChildren<OrbManager>().SetCanAttack(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerPresent = false;
            other.GetComponentInChildren<OrbManager>().SetCanAttack(true);
        }
    }

    void EnterTruck()
    {
        _playerObject.GetComponent<PlayerController>().ToggleIsActive();
        _mainCamera.transform.SetParent(_drivingCameraTransform);
        _playerObject.transform.SetParent(_drivingCameraTransform);

        //TODO: Cool disappear of character
        _playerObject.GetComponent<PlayerController>().Disappear();

        StartCoroutine(MoveCamera(_drivingCameraTransform));
    }

    public void ExitTruck()
    {
        _playerObject.transform.SetParent(null);
        _mainCamera.transform.SetParent(_playerCameraTransform);
        StartCoroutine(MoveCamera(_playerCameraTransform));
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
            _playerObject.GetComponent<PlayerController>().ToggleIsActive();
            
        }

        yield return null;
    }
}
