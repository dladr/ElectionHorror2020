using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : MonoBehaviour
{

    [SerializeField] private MimicMovement _mimicMovement;

    [SerializeField] private PostalBox _postalBox;

    [SerializeField] bool _hasActivated;

    // Start is called before the first frame update


    private void OnTriggerEnter(Collider other)
    {
        if (!_hasActivated && other.CompareTag("Player"))
        {
            _hasActivated = true;
            _mimicMovement.TrackTarget(other.transform);
        }
    }
}
