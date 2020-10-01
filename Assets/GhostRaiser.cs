using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRaiser : MonoBehaviour
{
    [SerializeField] Transform _ghostFrontTransform;
    [SerializeField] private SpriteColorManipulator _spriteColorManipulator;

    [SerializeField] private Transform _bodyFrontTransform;
    [SerializeField] private Transform _bodyTransform;

    [SerializeField] Transform[] _ghostTransformsToManipulate;

    private void Awake()
    {
        _ghostTransformsToManipulate = _ghostFrontTransform.GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        _spriteColorManipulator.UpdateSpriteRendererAlphas(0);
    }

    public void RaiseGhost()
    {
        MatchTransforms();
        _spriteColorManipulator.CallChangeAlphaOverTime(10, 100);
    }

    void MatchTransforms()
    {
        transform.position = _bodyTransform.position;
        transform.eulerAngles = _bodyTransform.eulerAngles;

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


}
