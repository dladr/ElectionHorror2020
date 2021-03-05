using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GhostRaiser : MonoBehaviour
{
    [SerializeField] Transform _ghostFrontTransform;
    [SerializeField] private SpriteColorManipulator _spriteColorManipulator;

    [SerializeField] private Transform _bodyFrontTransform;
    [SerializeField] private Transform _bodyTransform;
    private Quaternion _originalRotation;
    private Vector3 _originalPosition;
    private Quaternion _lerpStartRoation;
    private Vector3 _lerpStartPosition;

    public Vector3 PositionOffset;

    [SerializeField] Transform[] _ghostTransformsToManipulate;

    //[SerializeField] Vector3[] _initialEulers;
    private Quaternion[] _initialQuaternions;
    [SerializeField] private Vector3[] _initialPositions;

   // [SerializeField] private Vector3[] _lerpStartEulers;
   private Quaternion[] _lerpStartQuaternions;
    [SerializeField] private Vector3[] _lerpStartPositions;

    private bool _isMatchingTransforms;

    private void Awake()
    {
        _ghostTransformsToManipulate = _ghostFrontTransform.GetComponentsInChildren<Transform>();
       // _initialEulers = GetCurrentEulerAngles();
       _initialQuaternions = GetCurrentQuaternions();
        _initialPositions = GetCurrentPositions();
    }

    private void Start()
    {
       _spriteColorManipulator.UpdateSpriteRendererAlphas(0);
       // GetComponentInChildren<Animator>().Play("Empty");
       // MatchTransforms();
       _isMatchingTransforms = true;
    }

    private void LateUpdate()
    {
        if(_isMatchingTransforms)
            MatchTransforms();
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

Quaternion[] GetCurrentQuaternions()
{
    List<Quaternion> tempList = new List<Quaternion>();

    foreach (Transform transform1 in _ghostTransformsToManipulate)
    {
        tempList.Add(transform1.rotation);
    }

    return tempList.ToArray();
}

    Vector3[] GetCurrentPositions()
    {
        List<Vector3> templist = new List<Vector3>();

        foreach (Transform transform1 in _ghostTransformsToManipulate)
        {
            templist.Add(transform1.position);
        }

        return templist.ToArray();
    }
    public void RaiseGhost()
    {
        _isMatchingTransforms = true;
        _spriteColorManipulator.CallChangeAlphaOverTime(10, .5f);
        MatchTransforms();
       // _lerpStartEulers = GetCurrentEulerAngles();
       _lerpStartQuaternions = GetCurrentQuaternions();
        _lerpStartPositions = GetCurrentPositions();
        StartCoroutine(LerpEulersToGhostPosition(2, 5));
    }

    void MatchTransforms()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;

        transform.position = _bodyTransform.position;
        transform.rotation = _bodyTransform.rotation;

        Transform[] bodyfrontTransforms = _bodyFrontTransform.GetComponentsInChildren<Transform>();

        foreach (Transform transform1 in _ghostTransformsToManipulate)
        {
            foreach (Transform bodyfrontTransform in bodyfrontTransforms)
            {
                if (transform1.name == bodyfrontTransform.name)
                {
                    if(transform1.name.ToLower().Contains("torso") || transform1.name.ToLower().Contains("head"))
                       transform1.position = bodyfrontTransform.position;
                   // transform1.eulerAngles = bodyfrontTransform.eulerAngles;
                   transform1.rotation = bodyfrontTransform.rotation;
                }
            }
        }

        transform.position += PositionOffset;
        //transform.position += new Vector3(.001f, 0, -.003f);
    }

    IEnumerator LerpEulersToGhostPosition(float lerpTime, float delayTime)
    {
        _lerpStartPosition = transform.position;
        _lerpStartRoation = transform.rotation;

        float timeElapsed = 0;

        while (timeElapsed < delayTime)
        {
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        timeElapsed = 0;

        _isMatchingTransforms = false;

        while (timeElapsed < lerpTime)
        {
            timeElapsed += Time.deltaTime;
            float lerpPercent = timeElapsed / lerpTime;
            LerpInitialEulers(lerpPercent);
            yield return new WaitForEndOfFrame();
        }

        LerpInitialEulers(1);

        GetComponentInChildren<Animator>().enabled = true;
      //  GetComponent<Ghost>().ToggleIsActive();
      GetComponent<Ghost>().SetActive(true);

        yield return null;
    }
    void LerpInitialEulers(float lerpPercent)
    {
        for (int i = 0; i < _ghostTransformsToManipulate.Length; i++)
        {
            _ghostTransformsToManipulate[i].rotation =
                Quaternion.Lerp(_lerpStartQuaternions[i], _initialQuaternions[i], lerpPercent);

            Transform transform1 = _ghostTransformsToManipulate[i];

            if (transform1.name.ToLower().Contains("torso") || transform1.name.ToLower().Contains("head"))
            {
                _ghostTransformsToManipulate[i].position =
                    Vector3.Lerp(_lerpStartPositions[i], _initialPositions[i], lerpPercent);
            }
          
        }

        transform.position = Vector3.Lerp(_originalPosition, _lerpStartPosition, lerpPercent);
        transform.rotation = Quaternion.Lerp(_originalRotation, _lerpStartRoation, lerpPercent);
    }

}
