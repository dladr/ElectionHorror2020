using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string Dialogue;

    public Color Color;

    public FontStyles FontStyles;

    public bool HasTriggered;

    private TextModifier _textModifier;

    private void Awake()
    {
        _textModifier = SingletonManager.Get<TextModifier>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!HasTriggered && other.CompareTag("Truck"))
        {
            HasTriggered = true;
            _textModifier.UpdateTextTrio(Dialogue, Color, FontStyles);
            _textModifier.AutoTimeFades();
        }
    }

    public void Reset()
    {
        HasTriggered = false;
    }
}
