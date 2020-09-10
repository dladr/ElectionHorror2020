using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithXInput : MonoBehaviour
{
    [SerializeField] private float maxYRotation;

    [SerializeField] private Vector3 rotationVector3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateTransform(Input.GetAxis("Horizontal"));
    }

    void RotateTransform(float xInput)
    {
        transform.localEulerAngles = rotationVector3 * xInput * maxYRotation;
    }
}


