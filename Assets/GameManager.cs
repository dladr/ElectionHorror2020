using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<bool> BallotAnswers;

    public CheckPoint LastCheckPoint;

    public List<RotationReference> RotationReferences;

    // Start is called before the first frame update
    void Awake()
    {
       RotationReferences = new List<RotationReference>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLastCheckPoint(CheckPoint checkPoint)
    {
        LastCheckPoint = checkPoint;
    }

    public void AddRotationReference(RotationReference rotationReference)
    {
        if(!RotationReferences.Contains(rotationReference))
            RotationReferences.Add(rotationReference);
    }
}
