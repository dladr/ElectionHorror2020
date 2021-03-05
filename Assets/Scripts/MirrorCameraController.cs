using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCameraController : MonoBehaviour
{
    [SerializeField] private Camera _middleCamera;

    [SerializeField] private Camera _leftCamera;

    [SerializeField] private float _leftThreshold = -13;

    [SerializeField] private bool _isUsingMiddleCamera;

    [SerializeField] private RotateWithXInput _rotateWithXInput;

    [SerializeField] private bool _isLeft;

    private bool _hasLostMirror;

    [SerializeField] private GameObject _leftMirrorObject;

    [SerializeField] private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_hasLostMirror)
            return;

        if (!_isLeft && _rotateWithXInput._currentVector3.y <= _leftThreshold)
        {
            _isLeft = true;
            _leftCamera.enabled = true;
            _middleCamera.enabled = false;
        }

        if (_isLeft && _rotateWithXInput._currentVector3.y > _leftThreshold)
        {
            _isLeft = false;

            _leftCamera.enabled = false;

            if (_isUsingMiddleCamera)
                _middleCamera.enabled = true;
        }

    }

    public void LoseLeftMirror()
    {
        _audioSource.Play();
        _leftMirrorObject.SetActive(false);
        _hasLostMirror = true;
        _middleCamera.enabled = true;
        _middleCamera.farClipPlane = 90;
    }

    public void ResetLeftMirror()
    {
        _leftMirrorObject.SetActive(true);
        _hasLostMirror = false;
        _middleCamera.farClipPlane = 10;
    }
}
