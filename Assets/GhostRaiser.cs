using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostRaiser : MonoBehaviour
{
    [SerializeField] Transform _ghostFrontTransform;
    [SerializeField] private SpriteColorManipulator _spriteColorManipulator;

    [SerializeField] private Transform _bodyFrontTransform;
    [SerializeField] private Transform _bodyTransform;

    [SerializeField] Transform[] _ghostTransformsToManipulate;

    [SerializeField] Vector3[] _initialEulers;
    [SerializeField] private Vector3[] _initialPositions;

    [SerializeField] private Vector3[] _lerpStartEulers;
    [SerializeField] private Vector3[] _lerpStartPositions;

    private void Awake()
    {
        _ghostTransformsToManipulate = _ghostFrontTransform.GetComponentsInChildren<Transform>();
        _initialEulers = GetCurrentEulerAngles();
        _initialPositions = GetCurrentPositions();
    }

    private void Start()
    {
        _spriteColorManipulator.UpdateSpriteRendererAlphas(0);
    }

    Vector3[] GetCurrentEulerAngles()
    {
        List<Vector3> templist = new List<Vector3>();

        foreach (Transform transform1 in _ghostTransformsToManipulate)
        {
            templist.Add(transform1.eulerAngles);
        }

        return templist.ToArray();
    }

    Vector3[] GetCurrentPositions()
    {
        List<Vector3> templist = new List<Vector3>();

        foreach (Transform transform1 in _ghostTransformsToManipulate)
        {
            templist.Add(transform1.localPosition);
        }

        return templist.ToArray();
    }
    public void RaiseGhost()
    {
        MatchTransforms();
        _spriteColorManipulator.CallChangeAlphaOverTime(15, .75f);
        _lerpStartEulers = GetCurrentEulerAngles();
        _lerpStartPositions = GetCurrentPositions();
        StartCoroutine(LerpEulersToGhostPosition(15, 5));
    }

    void MatchTransforms()
    {
        transform.position = _bodyTransform.position;
        transform.rotation = _bodyTransform.rotation;

        Transform[] bodyfrontTransforms = _bodyFrontTransform.GetComponentsInChildren<Transform>();

        foreach (Transform transform1 in _ghostTransformsToManipulate)
        {
            foreach (Transform bodyfrontTransform in bodyfrontTransforms)
            {
                if (transform1.name == bodyfrontTransform.name)
                {
                    transform1.position = bodyfrontTransform.position;
                    transform1.eulerAngles = bodyfrontTransform.eulerAngles;
                }
            }
        }

        transform.position += new Vector3(.001f, 0, -.003f);
    }

    IEnumerator LerpEulersToGhostPosition(float lerpTime, float delayTime)
    {
        float timeElapsed = 0;

        while (timeElapsed < delayTime)
        {
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        timeElapsed = 0;

        while (timeElapsed < lerpTime)
        {
            timeElapsed += Time.deltaTime;
            float lerpPercent = timeElapsed / lerpTime;
            LerpInitialEulers(lerpPercent);
            yield return new WaitForEndOfFrame();
        }

        LerpInitialEulers(1);

        yield return null;
    }
    void LerpInitialEulers(float lerpPercent)
    {
        for (int i = 0; i < _ghostTransformsToManipulate.Length; i++)
        {
            _ghostTransformsToManipulate[i].eulerAngles =
                Vector3.Lerp(_lerpStartEulers[i], _initialEulers[i], lerpPercent);

            _ghostTransformsToManipulate[i].localPosition =
                Vector3.Lerp(_lerpStartPositions[i], _initialPositions[i], lerpPercent);
        }
    }

}
