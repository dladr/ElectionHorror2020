using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using UnityEngine;

public class TogglePlayerRotation : MonoBehaviour
{
    public bool IsTurningOn;

    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = SingletonManager.Get<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerController.IsUpdatingRotation = IsTurningOn;
    }
}
