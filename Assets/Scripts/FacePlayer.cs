using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private Transform _playerTransform;

    private bool _isRotatingToFacePlayer;

    private Quaternion _initialRotation;

    // Start is called before the first frame update
    void Awake()
    {
        _initialRotation = transform.rotation;
        _playerTransform = SingletonManager.Get<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isRotatingToFacePlayer)
            RotateToFacePlayer();
    }

    void RotateToFacePlayer()
    {
       transform.LookAt(_playerTransform);
       transform.Rotate(0, 180, 0);

    }
    public void StartRotating()
    {
        _isRotatingToFacePlayer = true;
    }

    public void Reset()
    {
        _isRotatingToFacePlayer = false;
        transform.rotation = _initialRotation;
    }
}
