using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using UnityEngine;

public class TruckAttractor : MonoBehaviour
{
   
    public void AttractTruck()
    {
        Transform truckTransform = SingletonManager.Get<TruckMovement>().transform;
        truckTransform.position = transform.position;
        truckTransform.rotation = transform.rotation;
    }

   
}
