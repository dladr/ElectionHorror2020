﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using TMPro;
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
    public Transform PoliceCarStartTransform;
    public string CaptureMessage;
    public string TruckHiddenMessage;
    public Color TextColorHidden;
    public Color TextColor;
    public FontStyles FontStyles;
    public FontStyles FontStylesHidden;

    private TextModifier _textModifier;

    private bool _hasPassedPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        _truckMovement = SingletonManager.Get<TruckMovement>();
        _truckTransform = _truckMovement.transform;
        _textModifier = SingletonManager.Get<TextModifier>();
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
        _truckMovement.IsCopNearby = true;
    }

    public void Deactivate()
    {
        PoliceCar.SetActive(false);
        IsActive = false;
        _truckMovement.IsCopNearby = false;
    }

    void MovePoliceCar()
    {
        PoliceCar.transform.position =
            Vector3.MoveTowards(PoliceCar.transform.position, PoliceCarDestination.position, Time.deltaTime * Speed);

        if (_hasPassedPlayer && Vector3.Distance(PoliceCar.transform.position, _truckTransform.position) < CaptureDistance)
        {
            _hasPassedPlayer = true;

            if (_truckMovement.IsHidden)
            {
                _textModifier.UpdateTextTrio(TruckHiddenMessage, TextColorHidden, FontStylesHidden);
                _textModifier.AutoTimeFades();
                _truckMovement.CaughtByCops();
            }

            else
            {
                _textModifier.UpdateTextTrio(CaptureMessage, TextColor, FontStyles);
                _textModifier.AutoTimeFades();
                _truckMovement.CaughtByCops();
            }
        }

        if (Vector3.Distance(PoliceCar.transform.position, PoliceCarDestination.position) < .1f)
        {
            Deactivate();
        }

    }

    public void Reset()
    {
        Deactivate();
        PoliceCar.transform.position = PoliceCarStartTransform.position;
    }
}
