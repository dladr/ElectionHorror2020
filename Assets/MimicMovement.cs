using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
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

    public bool IsMovingRightLeg;
    // Start is called before the first frame update
    void Awake()
    {
        TargetTransform = SingletonManager.Get<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveNextLeg()
    {

    }

    IEnumerator MoveLeftLeg(float degreesToRotate)
    {
        FrontLeftTransform.position = FrontLeftReferenceTransform.position;
        FrontLeftTransform.rotation = FrontLeftReferenceTransform.rotation;

        transform.parent = FrontLeftTransform;

        float timeElapsed = 0;
        Vector3 startingEulers = FrontLeftTransform.eulerAngles;
        Vector3 targetEulers = new Vector3(startingEulers.x, -degreesToRotate, startingEulers.z);
        Vector3 midpointEulers = new Vector3(XDegreesToRotate, -degreesToRotate/2, ZDegreesToRotate);
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
        Vector3 targetEulers = new Vector3(startingEulers.x, degreesToRotate, startingEulers.z);
        Vector3 midpointEulers = new Vector3(-XDegreesToRotate, degreesToRotate / 2, -ZDegreesToRotate);
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

        MoveNextLeg();
        yield return null;
    }
}
