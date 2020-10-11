﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

public class CrashedCar : MonoBehaviour
{
    public string Title;

    public string[] DialogueStrings;

    public Color[] TextColors;

    public FontStyles[] FontStyles;

    private TextModifier _textModifier;

    private OrbManager _orbManager;

    private bool _isPlayerPresent;

    private ScreenFader _screenFader;

    public Transform _playerLocation;
    public Transform _truckLocation;
    public GameObject TruckGameObject;

    private RearDoor _rearDoor;

    private bool _hasPlayerHitSpace;

    private PlayerController _playerController;



    // Start is called before the first frame update
    void Awake()
    {
        _textModifier = SingletonManager.Get<TextModifier>();
        _orbManager = SingletonManager.Get<OrbManager>();
        _screenFader = SingletonManager.Get<ScreenFader>();
        _rearDoor = SingletonManager.Get<RearDoor>();
        _playerController = SingletonManager.Get<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Action"))
            _hasPlayerHitSpace = true;

        if (_isPlayerPresent && Input.GetButtonDown("Action"))
        {
            StartCoroutine(CrashSequence());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerPresent = true;
            _textModifier.UpdateTextTrio(Title, TextColors[0], FontStyles[0]);
            _textModifier.Fade(true, 10);
            _orbManager.SetCanAttack(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerPresent = false;
            _textModifier.Fade(false, 10);
            _orbManager.SetCanAttack(true);
        }
    }

    IEnumerator CrashSequence()
    {
        _playerController.Deactivate();

        _textModifier.UpdateTextTrio(DialogueStrings[0], TextColors[0], FontStyles[0]);
        _textModifier.Fade();
        _hasPlayerHitSpace = false;
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(DialogueStrings[1], TextColors[1], FontStyles[1]);
        _textModifier.Fade();
        _hasPlayerHitSpace = false;
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(DialogueStrings[2], TextColors[2], FontStyles[2]);
        _textModifier.Fade();
        _hasPlayerHitSpace = false;
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _screenFader.Fade(isFadingIn:false);

        TruckGameObject.transform.position = _truckLocation.position;
        TruckGameObject.transform.rotation = _truckLocation.rotation;
        _playerController.transform.position = _playerLocation.position;
        _rearDoor.ActivateStranger(true);

        yield return new WaitForSeconds(.5f);

        _screenFader.Fade();


        yield return null;
    }
}