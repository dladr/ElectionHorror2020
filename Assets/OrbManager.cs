using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    [SerializeField] private Transform[] _orbTransforms;
    [SerializeField] private Orb[] _orbs;

    [SerializeField] private int _numberUnlocked;

    [SerializeField] private float _expansionSpeed;

    [SerializeField] private float _spinSpeed;

    [SerializeField] private float _numberOfRotations;

    [SerializeField] private float _coolDownTime;

    [SerializeField] private bool _canAttack;

    [SerializeField] private bool _externalCanAttack;

    // Start is called before the first frame update
    void Awake()
    {
        _numberUnlocked = 1;
        _canAttack = _externalCanAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public void UnlockOrb()
    {
        if (_numberUnlocked > _orbTransforms.Length - 1)
            return;

        _orbTransforms[_numberUnlocked].gameObject.SetActive(true);
        _numberUnlocked++;
    }

    public void SetCanAttack(bool canAttack)
    {
        _externalCanAttack = canAttack;
    }

    [Button]
    public void StartAttack()
    {
        if (!_canAttack || !_externalCanAttack)
            return;

        _canAttack = false;

        StartCoroutine(Attack());
    }

    public void HideOrbs(bool isHiding)
    {
        if (isHiding)
        {
            foreach (Transform orbTransform in _orbTransforms)
            {
                orbTransform.GetComponent<Renderer>().enabled = false;
            }
        }

        else
        {
            foreach (Transform orbTransform in _orbTransforms)
            {
                orbTransform.GetComponent<Renderer>().enabled = true;
            }
        }
    }

    IEnumerator Attack()
    {
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
                orbDirection = -1;

            foreach (Transform orbTransform in _orbTransforms)
            {
                orbTransform.position += orbTransform.forward * orbDirection * _expansionSpeed * Time.deltaTime;
            }

            yield return new WaitForEndOfFrame();
        }

        foreach (Transform orbTransform in _orbTransforms)
        {
            orbTransform.localPosition = Vector3.zero;
        }

        foreach (Orb orb in _orbs)
        {
            orb.Deactivate();
        }

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

        yield return null;
    }
}
