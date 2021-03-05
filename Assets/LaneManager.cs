using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class LaneManager : MonoBehaviour
{
    [SerializeField] private CarController[] _carControllers;

    public List<CarController> ReadyCarControllers;

    [SerializeField] public LaneBraker _laneBraker;

    [SerializeField] private bool _isActive;

    public float TimeBetweenCopCars;

    [SerializeField] private FaillingRail[] _rails;

    private bool _isWaitingForFinalWave;

    private TextModifier _textModifier;
    [SerializeField] private String FinalWarning;
    [SerializeField] private Color _textColor;


    private void Awake()
    {
        _textModifier = SingletonManager.Get<TextModifier>();
    }

    public void Activate()
    {
        _laneBraker.IsActive = true;
        _isActive = true;

        ReadyCarControllers = _carControllers.ToList();

        foreach (CarController readyCarController in ReadyCarControllers)
        {
            readyCarController._laneManager = this;
        }

        StartRandomCar();
    }

    public void Deactivate()
    {
        _laneBraker.IsActive = false;
        _isActive = false;
        _isWaitingForFinalWave = false;
        StopAllCoroutines();
    }

    public void Reset()
    {
        Deactivate();

       _laneBraker.Reset();

       foreach (CarController carController in _carControllers)
       {
           carController.Reset();
       }

        foreach (FaillingRail faillingRail in _rails)
        {
            faillingRail.Reset();
        }
    }

    void StartRandomCar()
    {
        if (!_isActive)
            return;

        if (ReadyCarControllers.Count > 0)
        {
            int indexToStart = Random.Range(min: 0, max: ReadyCarControllers.Count);
            ReadyCarControllers[index: indexToStart].StartMoving();
        }

        Invoke(methodName: nameof(StartRandomCar), time: TimeBetweenCopCars);
    }

    public void RemoveCarFromReadyList(CarController carController)
    {
        ReadyCarControllers.Remove(item: carController);
    }

    public void AddCarToReadyList(CarController carController)
    {
        if(!ReadyCarControllers.Contains(item: carController))
             ReadyCarControllers.Add(item: carController);
    }

    public void StartFinalWave()
    {
        StopAllCoroutines();
        _isWaitingForFinalWave = true;
        _isActive = false;

        foreach (CarController carController in _carControllers)
        {
            carController.SetupFinalWave();
        }

        _textModifier.UpdateTextTrio(FinalWarning, _textColor, FontStyles.Bold);
        _textModifier.AutoTimeFades();

        AttemptFinalWave();
    }

    void AttemptFinalWave()
    {
        if (!_isWaitingForFinalWave)
            return;

        if (ReadyCarControllers.Count == 4)
        {
            _isWaitingForFinalWave = false;
            foreach (CarController carController in _carControllers)
            {
                carController.SetIsReleasingConstraints(true);
                carController.StartMoving();
            }
        }

        else
        {
            Invoke(nameof(AttemptFinalWave), time: .5f);
        }
    }
}
