﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

public class EnvelopePickup : MonoBehaviour
{
    private bool _isPlayerPresent;

    private TextModifier _textModifier;

    private OrbManager _orbManager;
    // Start is called before the first frame update
    void Awake()
    {
        _textModifier = SingletonManager.Get<TextModifier>();
        _orbManager = SingletonManager.Get<OrbManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_isPlayerPresent && Input.GetButtonDown("Action"))
            Pickup();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerPresent = true;
            _textModifier.UpdateTextTrio("Gram Gram's Ballot", Color.white, FontStyles.Normal);
            _textModifier.Fade(true, 10);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerPresent = false;
            _textModifier.Fade(false, 10);
        }
    }

    private void Pickup()
    {
        
        _textModifier.UpdateText("Picked up Gram Gram's ballot.");
        _textModifier.AutoTimeFades();
       SingletonManager.Get<OrbManager>().UnlockOrb();
        Destroy(gameObject);
    }
}
