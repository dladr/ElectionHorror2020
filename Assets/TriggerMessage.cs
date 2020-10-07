using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

public class TriggerMessage : MonoBehaviour
{
    public string Title;

    public string Description;

    public Color TextColor;

    public FontStyles FontStyles;

    private TextModifier _textModifier;

    private OrbManager _orbManager;

    private bool _isPlayerPresent;

    // Start is called before the first frame update
    void Awake()
    {
        _textModifier = SingletonManager.Get<TextModifier>();
        _orbManager = SingletonManager.Get<OrbManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerPresent && Input.GetButtonDown("Action"))
        {
            _textModifier.UpdateTextTrio(Description, TextColor, FontStyles);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerPresent = true;
            _textModifier.UpdateTextTrio(Title, TextColor, FontStyles);
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
}
