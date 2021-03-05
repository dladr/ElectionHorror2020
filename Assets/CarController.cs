using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class CarController : MonoBehaviour
{
    public bool IsMoving;

    public Transform StartingTransform;

    public Transform EndingTransform;

    public float DefaultSpeed = 5;

    public float FastSpeed = 8;

    public float CurrentSpeed;

    public float DistanceToEnd;

    public float DistanceToSpeedUp;

    public float DistanceToReset;

    public bool HasSpedUp;

    public Transform CarTransform;

    public GameObject CarGameObject;

    [SerializeField] public LaneManager _laneManager;
    [SerializeField] private TruckParenter _truckParenter;

    private bool _isMovingForward;
   
    void Awake()
    {
        CurrentSpeed = DefaultSpeed;
        _truckParenter = CarGameObject.GetComponentInChildren<TruckParenter>();

    }

   
    void Update()
    {
        if (!IsMoving)
            return;

        SetDistance();

        if (!HasSpedUp && DistanceToEnd < DistanceToSpeedUp)
        {
            HasSpedUp = true;
            CurrentSpeed = FastSpeed;
        }

        if (DistanceToEnd < DistanceToReset)
        {
            if (_laneManager._laneBraker.HasReachedFinalPosition)
            {
                IsMoving = false;
                SingletonManager.Get<GameManager>().CountDownReset();
            }

            else
            {
                Reset();
            }
           
            return;
        }

        MoveCar();


    }

    void SetDistance()
    {
        if(_isMovingForward)
          DistanceToEnd = Vector3.Distance(CarTransform.position, EndingTransform.position);
        else
        {
            DistanceToEnd = Vector3.Distance(CarTransform.position, StartingTransform.position);
        }
    }

    void DetermineFastestEnd()
    {
        float distanceToEnd = Vector3.Distance(CarTransform.position, EndingTransform.position);
        float distanceToStart = DistanceToEnd = Vector3.Distance(CarTransform.position, StartingTransform.position);

        if (distanceToStart < distanceToEnd)
        {
            _isMovingForward = false;
            _truckParenter.ReleaseTruck();
        }

        CurrentSpeed = FastSpeed;
    }

    void MoveCar()
    {
        if(_isMovingForward)
         CarTransform.position =
            Vector3.MoveTowards(CarTransform.position, EndingTransform.position, CurrentSpeed * Time.deltaTime);
        else
        {
            CarTransform.position =
                Vector3.MoveTowards(CarTransform.position, StartingTransform.position, CurrentSpeed * Time.deltaTime);
        }
    }

    public void SetIsReleasingConstraints(bool isReleasing)
    {
        _truckParenter.IsReleasingConstraints = isReleasing;
    }

    public void SetupFinalWave()
    {
       // SetIsReleasingConstraints(true);
        DetermineFastestEnd();
    }

    public void Reset()
    {
        HasSpedUp = false;
        CurrentSpeed = DefaultSpeed;
        _truckParenter.ReleaseTruck();
        CarTransform.position = StartingTransform.position;
        IsMoving = false;
        CarGameObject.SetActive(false);
        _laneManager.AddCarToReadyList(this);
       SetIsReleasingConstraints(false);
       _isMovingForward = true;

    }

    public void StartMoving()
    {
        CarTransform.position = StartingTransform.position;
        CarGameObject.SetActive(true);
        IsMoving = true;
        _laneManager.RemoveCarFromReadyList(this);

    }
}
