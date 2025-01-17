﻿using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    [SerializeField] private Transform[] _orbTransforms;
    [SerializeField] private Orb[] _orbs;

    [SerializeField] public int _numberUnlocked;

    [SerializeField] private float _expansionSpeed;

    [SerializeField] private float _spinSpeed;

    [SerializeField] private float _numberOfRotations;

    [SerializeField] private float _coolDownTime;

    [SerializeField] private bool _canAttack;

    [SerializeField] private bool _externalCanAttack;
    [SerializeField] private Animator _playerAnimator;
    private PlayerController _playerController;

    [SerializeField] private AudioSource _audioSource;

    public Ghost CowardlyGhost;
    public bool IsCowardlyGhostNear;
    public bool IsCowardlyGhostWaiting;
    public float IdleOrbDistance;
    public bool IsIdleRotating;
    public Vector3 DefaultOrbScale;

    // Start is called before the first frame update
    void Awake()
    {
        _numberUnlocked = 0;
        _canAttack = _externalCanAttack = true;
        _playerController = GetComponentInParent<PlayerController>();
        _audioSource = GetComponent<AudioSource>();

        foreach (Transform orbTransform in _orbTransforms)
        {
            //orbTransform.localPosition = Vector3.zero;
            orbTransform.localPosition = orbTransform.forward * IdleOrbDistance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsIdleRotating)
            IdleRotation();
    }

    [Button]
    public void UnlockOrb()
    {
        if (_numberUnlocked > _orbTransforms.Length - 1)
            return;

        _orbTransforms[_numberUnlocked].gameObject.SetActive(true);
        _numberUnlocked++;

        AdjustOrbAngles();
    }

    void AdjustOrbAngles()
    {
        float degreeOffset = 0;

        if(_numberUnlocked > 0)
         degreeOffset = 360 / _numberUnlocked;

        for (int i = 0; i < _numberUnlocked; i++)
        {
            _orbTransforms[i].localEulerAngles = new Vector3(0, degreeOffset * i, 0);
        }
    }

    void RemoveOrb()
    {
        if (_numberUnlocked == 0)
            return;

        _orbTransforms[_numberUnlocked - 1].gameObject.SetActive(false);
        _numberUnlocked--;

        AdjustOrbAngles();
    }

    public void SetCanAttack(bool canAttack)
    {
        _externalCanAttack = canAttack;
    }

    [Button]
    public void StartAttack(bool isDroppingBag = true)
    {
        if (!_canAttack || !_externalCanAttack || _numberUnlocked < 1)
            return;

        _canAttack = false;

        StartCoroutine(Attack(isDroppingBag));
    }

    public void HideOrbs(bool isHiding)
    {
        if (isHiding)
        {
            foreach (Transform orbTransform in _orbTransforms)
            {
                orbTransform.GetComponent<Renderer>().enabled = false;
            }

            foreach (Orb orb in _orbs)
            {
                orb.TurnOffLights();
            }
        }

        else
        {
            foreach (Transform orbTransform in _orbTransforms)
            {
                orbTransform.GetComponent<Renderer>().enabled = true;
            }

            foreach (Orb orb in _orbs)
            {
                orb.TurnOnLights();
            }
        }
    }

    public void SetNumberOfOrbs(int numberOfOrbs)
    {
        if (numberOfOrbs == _numberUnlocked)
            return;

        int difference = numberOfOrbs - _numberUnlocked;

        if (difference > 0)
        {
            for (int i = 0; i < difference; i++)
            {
                UnlockOrb();
            }
        }

        else
        {
            for (int i = 0; i < -difference; i++)
            {
                RemoveOrb();
            }
        }


    }

    public bool CheckInternalCanAttack()
    {
        return _canAttack;
    }

    void IdleRotation()
    {
        transform.Rotate(0, _spinSpeed * _numberOfRotations * Time.deltaTime, 0);
    }

    IEnumerator Attack(bool isDroppingBag = true)
    {
        IsIdleRotating = false;

        _audioSource.Play();

        if (isDroppingBag)
        {
            _playerController.DropBag();
            _playerAnimator.SetBool("IsHoldingEnvelope", true);
        }

        else
        {
            if (IsCowardlyGhostWaiting)
            {
                IsCowardlyGhostWaiting = false;
                IsCowardlyGhostNear = true;
            }
        }
        

        foreach (Orb orb in _orbs)
        {
            orb.Arm();
            orb.Reactivate();
        }

        float degreesRotated = 0;

        int orbDirection = 1;

        while (degreesRotated < _numberOfRotations * 360)
        {
            degreesRotated += _spinSpeed * _numberOfRotations * Time.deltaTime;
            transform.Rotate(0, _spinSpeed * _numberOfRotations * Time.deltaTime, 0);

            if (degreesRotated >= _numberOfRotations * 180)
            {
                orbDirection = -1;
                if (IsCowardlyGhostNear)
                {
                    IsCowardlyGhostNear = false;
                    CowardlyGhost.TakeCowardlyDamage();
                    foreach (Orb orb in _orbs)
                    {
                        orb.DeactivateWithParticles();
                    }
                    CowardlyGhost = null;
                }
            }
                

            foreach (Transform orbTransform in _orbTransforms)
            {
                orbTransform.position += orbTransform.forward * orbDirection * _expansionSpeed * Time.deltaTime;
            }

            yield return new WaitForEndOfFrame();
        }

        foreach (Transform orbTransform in _orbTransforms)
        {
            //orbTransform.localPosition = Vector3.zero;
            orbTransform.localPosition = orbTransform.forward * IdleOrbDistance;
        }

        foreach (Orb orb in _orbs)
        {
            orb.Deactivate();
        }

        _playerAnimator.SetBool("IsHoldingEnvelope", false);

        float timeElapsed = 0;

        while (timeElapsed < _coolDownTime)
        {
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _canAttack = true;

        foreach (Orb orb in _orbs)
        {
           orb.Reactivate();
           orb.Disarm();
        }

        if(!_playerController.IsActive)
            HideOrbs(true);

        IsIdleRotating = true;

        yield return null;
    }

    public void SetCowardlyGhost(Ghost ghost)
    {
        CowardlyGhost = ghost;
        IsCowardlyGhostWaiting = true;
    }

    public void GhostBossAttack()
    {
        foreach (Orb orb in _orbs)
        {
            orb.DeactivateWithParticles();
        }

        RemoveOrb();
    }
}
