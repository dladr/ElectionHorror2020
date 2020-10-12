using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Unity.Profiling;
using UnityEngine;

public class CopZone : MonoBehaviour
{
    private TruckMovement _truckMovement;
    private Transform _truckTransform;
    [SerializeField] private GameObject PoliceCar;
    [SerializeField] private Transform PoliceCarDestination;
    [SerializeField] private float Speed;
    [SerializeField] private float CaptureDistance;
    public float Distance;
    public bool IsActive;
    public bool ShowDistance;

    // Start is called before the first frame update
    void Awake()
    {
        _truckMovement = SingletonManager.Get<TruckMovement>();
        _truckTransform = _truckMovement.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (ShowDistance)
        {
            Distance = Vector3.Distance(PoliceCar.transform.position, _truckTransform.position);
        }

        if(IsActive)
            MovePoliceCar();
    }

    public void Activate()
    {
        PoliceCar.SetActive(true);
        IsActive = true;
    }

    public void Deactivate()
    {
        PoliceCar.SetActive(false);
        IsActive = false;
    }

    void MovePoliceCar()
    {
        PoliceCar.transform.position =
            Vector3.MoveTowards(PoliceCar.transform.position, PoliceCarDestination.position, Time.deltaTime * Speed);

        if (Vector3.Distance(PoliceCar.transform.position, _truckTransform.position) < CaptureDistance)
        {
            if (_truckMovement.IsHidden)
            {
                Debug.Log("Successful hide!");
            }

            else
            {
                Debug.Log("Cop Caught you!");
            }
        }

        if (Vector3.Distance(PoliceCar.transform.position, PoliceCarDestination.position) < .1f)
        {
            Deactivate();
        }

    }
}
