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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
}
