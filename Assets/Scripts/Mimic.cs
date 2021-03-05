using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using UnityEngine;

public class Mimic : MonoBehaviour
{

    [SerializeField] private MimicMovement _mimicMovement;

    [SerializeField] private PostalBox _postalBox;

    [SerializeField] bool _hasActivated;
    [SerializeField] private bool _isInvincible;
    [SerializeField] private float _iFramesTime;
    [SerializeField] private float _deadlyDelay;
    [SerializeField] private bool _isDeadly;
    [SerializeField] private bool _isPlayerPresent;
    private PlayerController _playerController;

    [SerializeField] private Ghost[] _ghosts;

    private int ghostIndex;


    private void Awake()
    {
        _postalBox.IsDeactivated = true;
    }

    private void Update()
    {
        if (_isPlayerPresent && _isDeadly)
        {
            _playerController.TakeDamage();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!_hasActivated && other.CompareTag("Player"))
        {
            _playerController = other.GetComponent<PlayerController>();
            _hasActivated = true;
            _mimicMovement.TrackTarget();
            Invoke(nameof(BecomeDeadly), _deadlyDelay);
        }

        else if (other.CompareTag("Player"))
        {
            _isPlayerPresent = true;
        }

        else  if (!_isInvincible && other.CompareTag("Orb"))
        {
            if (!_hasActivated)
            {
                _playerController = SingletonManager.Get<PlayerController>();
                _hasActivated = true;
                _mimicMovement.TrackTarget();
                Invoke(nameof(BecomeDeadly), _deadlyDelay);
            }

            if (ghostIndex < _ghosts.Length)
            {
                other.GetComponent<Orb>().DeactivateWithParticles();
                TakeHit();
            }

            

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _isPlayerPresent = false;
    }

    void BecomeDeadly()
    {
        _isDeadly = true;
    }

    void TakeHit()
    {
        _ghosts[ghostIndex].transform.position = transform.position;
        _ghosts[ghostIndex].ToggleIsActive();
        ghostIndex++;

        if(ghostIndex >= _ghosts.Length)
            Die();
        else
        {
            _isInvincible = true;

            foreach (MaterialSetter materialSetter in _postalBox._materialSetters)
            {
                materialSetter.TurnOffGlow();
            }

            Invoke(nameof(TurnOffInvincibility), _iFramesTime);

        }
    }

    void TurnOffInvincibility()
    {
        _isInvincible = false;

        foreach (MaterialSetter materialSetter in _postalBox._materialSetters)
        {
            materialSetter.TurnOnGlow();
        }

    }

    void Die()
    {
        _mimicMovement.Stop();
        _isDeadly = false;
        _postalBox.IsDeactivated = false;
    }

    public void Reset()
    {

        ghostIndex = 0;
        _hasActivated = false;
        _isDeadly = false;
        _postalBox.IsDeactivated = true;
        _mimicMovement.Reset();

    }
}
