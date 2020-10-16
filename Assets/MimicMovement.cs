using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Sirenix.OdinInspector;
using UnityEngine;

public class MimicMovement : MonoBehaviour
{
    [SerializeField] private Transform FrontRightTransform;

    [SerializeField] private Transform FrontRightReferenceTransform;

    [SerializeField] private Transform FrontLeftTransform;

    [SerializeField] private Transform FrontLeftReferenceTransform;

    [SerializeField] private Transform TargetTransform;

    [SerializeField] private Transform OriginalTransform;

    [SerializeField] private float TimeToRotate;

    [SerializeField] private float XDegreesToRotate;

    [SerializeField] private float ZDegreesToRotate;

    [SerializeField] private float StandardDegreesToRotate;
    [SerializeField] private Transform ReferenceAngle;

    private List<Vector3> _originalPositions;
    private List<Quaternion> _originalRotations;

    public bool IsMovingRightLeg;

    private bool _isTrackingTarget;

    public float LookRotation;

    private void Awake()
    {
        TargetTransform = SingletonManager.Get<PlayerController>().transform;
        _originalPositions = new List<Vector3>();
        _originalRotations = new List<Quaternion>();

        _originalPositions.Add(FrontRightTransform.localPosition);
        _originalPositions.Add(FrontRightReferenceTransform.localPosition);
        _originalPositions.Add(FrontLeftTransform.localPosition);
        _originalPositions.Add(FrontLeftReferenceTransform.localPosition);
        _originalPositions.Add(transform.localPosition);

        _originalRotations.Add(FrontRightTransform.localRotation);
        _originalRotations.Add(FrontRightReferenceTransform.localRotation);
        _originalRotations.Add(FrontLeftTransform.localRotation);
        _originalRotations.Add(FrontLeftReferenceTransform.localRotation);
        _originalRotations.Add(transform.localRotation);
    }

    public void TrackTarget()
    {
        _isTrackingTarget = true;
        MoveNextLeg();
    }

    public void Stop()
    {
        _isTrackingTarget = false;
    }

    public void Reset()
    {
        StopAllCoroutines();
        IsMovingRightLeg = false;

        _isTrackingTarget = false;
        transform.parent = OriginalTransform;

        FrontRightTransform.localPosition = _originalPositions[0];
        FrontRightTransform.localRotation = _originalRotations[0];

        FrontRightReferenceTransform.localPosition = _originalPositions[1];
        FrontRightReferenceTransform.localRotation = _originalRotations[1];

        FrontLeftTransform.localPosition = _originalPositions[2];
        FrontLeftTransform.localRotation = _originalRotations[2];

        FrontLeftReferenceTransform.localPosition = _originalPositions[3];
        FrontLeftReferenceTransform.localRotation = _originalRotations[3];

        transform.localPosition = _originalPositions[4];
        transform.localRotation = _originalRotations[4];

        ReferenceAngle.localEulerAngles = Vector3.zero;
        ReferenceAngle.localPosition = Vector3.zero;


    }

    [Button]
    void MoveNextLeg()
    {
        if (!_isTrackingTarget)
            return;

        //Vector3 translation = TargetTransform.position - transform.position;
        //Quaternion lookRotation = Quaternion.LookRotation(translation, Vector3.up);
        //float lookRotationY = lookRotation.eulerAngles.y;
        ReferenceAngle.localEulerAngles = Vector3.zero;
        ReferenceAngle.LookAt(TargetTransform);
        float lookRotationY = ReferenceAngle.localEulerAngles.y;
        lookRotationY -= 90;

        if (lookRotationY > 180)
            lookRotationY -= 360;

        LookRotation = lookRotationY;
       // lookRotationY = 0;

        float degreesToRotate = StandardDegreesToRotate;
        if (lookRotationY > 0 && IsMovingRightLeg)
        {
            degreesToRotate += lookRotationY;
        }

        if (lookRotationY < 0 && !IsMovingRightLeg)
        {
            degreesToRotate -= lookRotationY;
        }

        

        if (IsMovingRightLeg)
           StartCoroutine(MoveRightLeg(degreesToRotate)) ;

        else
        {
           StartCoroutine(MoveLeftLeg(degreesToRotate)) ;
        }

    }

    IEnumerator MoveLeftLeg(float degreesToRotate)
    {
        FrontLeftTransform.position = FrontLeftReferenceTransform.position;
        FrontLeftTransform.rotation = FrontLeftReferenceTransform.rotation;

        transform.parent = FrontLeftTransform;

        float timeElapsed = 0;
        Vector3 startingEulers = FrontLeftTransform.eulerAngles;
        Vector3 targetEulers = new Vector3(startingEulers.x, startingEulers.y -degreesToRotate, startingEulers.z);
        Vector3 midpointEulers = new Vector3(XDegreesToRotate, startingEulers.y - (degreesToRotate/2), ZDegreesToRotate);
        while (timeElapsed < TimeToRotate)
        {
            timeElapsed += Time.deltaTime;
            var lerpPercent = timeElapsed / TimeToRotate; 
            if(lerpPercent < .5f)
                FrontLeftTransform.eulerAngles =  Vector3.Lerp(startingEulers, midpointEulers, lerpPercent * 2);

            else
            {
                FrontLeftTransform.eulerAngles = Vector3.Lerp(midpointEulers, targetEulers, (lerpPercent-.5f) * 2);
            }

            yield return new WaitForEndOfFrame();
        }

        FrontLeftTransform.eulerAngles = targetEulers;

        IsMovingRightLeg = true;

        MoveNextLeg();
        yield return null;
    }

    IEnumerator MoveRightLeg(float degreesToRotate)
    {
        FrontRightTransform.position = FrontRightReferenceTransform.position;
        FrontRightTransform.rotation = FrontRightReferenceTransform.rotation;

        transform.parent = FrontRightTransform;

        float timeElapsed = 0;
        Vector3 startingEulers = FrontRightTransform.eulerAngles;
        Vector3 targetEulers = new Vector3(startingEulers.x, startingEulers.y + degreesToRotate, startingEulers.z);
        Vector3 midpointEulers = new Vector3(-XDegreesToRotate, startingEulers.y + (degreesToRotate / 2), -ZDegreesToRotate);
        while (timeElapsed < TimeToRotate)
        {
            timeElapsed += Time.deltaTime;
            var lerpPercent = timeElapsed / TimeToRotate;
            if (lerpPercent < .5f)
                FrontRightTransform.eulerAngles = Vector3.Lerp(startingEulers, midpointEulers, lerpPercent * 2);

            else
            {
                FrontRightTransform.eulerAngles = Vector3.Lerp(midpointEulers, targetEulers, (lerpPercent - .5f) * 2);
            }

            yield return new WaitForEndOfFrame();
        }

        FrontRightTransform.eulerAngles = targetEulers;

        IsMovingRightLeg = false;

        MoveNextLeg();
        yield return null;
    }
}
