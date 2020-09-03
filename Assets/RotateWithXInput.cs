using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithXInput : MonoBehaviour
{
    [SerializeField] private float maxYRotation;
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
        transform.localEulerAngles = new Vector3(0, xInput * maxYRotation, 0);
    }
}
