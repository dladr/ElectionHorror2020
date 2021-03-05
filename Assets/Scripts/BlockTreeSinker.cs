using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTreeSinker : MonoBehaviour
{
    [SerializeField] private MoveTreeVertical _moveTreeVertical;
    [SerializeField] private Collider _collider;
    private bool _hasTriggered;
    private bool _isWaiting;
    [SerializeField] private AudioSource _audioSource;


    private void Update()
    {
        if (_isWaiting)
        {
            if (!_moveTreeVertical.IsMoving)
            {
                _isWaiting = false;
                _collider.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasTriggered && other.CompareTag("Player"))
        {
            _hasTriggered = true;
            _moveTreeVertical.MoveVertical(false);
            _isWaiting = true;
            _audioSource.Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(!_hasTriggered)
            OnTriggerEnter(other);
    }

    public void ExternalTrigger()
    {
        _hasTriggered = true;
        _moveTreeVertical.MoveVertical(false);
        _isWaiting = true;
    }
}
