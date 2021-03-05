using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTreeRaiser : MonoBehaviour
{
    [SerializeField] private MoveTreeVertical _moveTreeVertical;
    [SerializeField] private Collider _collider;
   [SerializeField] private bool _hasTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasTriggered && other.CompareTag("Player"))
        {
            _hasTriggered = true;
            _moveTreeVertical.MoveVertical(true);
            _collider.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_hasTriggered)
            OnTriggerEnter(other);
    }
}
