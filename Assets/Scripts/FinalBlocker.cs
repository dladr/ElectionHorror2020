using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

public class FinalBlocker : MonoBehaviour
{
    private TextModifier _textModifier;

   [SerializeField] InvisiblePlayer _invisiblePlayer;

    [SerializeField] private Collider _wallCollider;

    [SerializeField] private FinalAreaSequence _finalAreaSequence;

    private void Awake()
    {
        _textModifier = SingletonManager.Get<TextModifier>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InvisiblePlayer"))
        {
            if (_invisiblePlayer.HasCollectedBag)
            {
                _wallCollider.enabled = false;
                _invisiblePlayer.StartAutoWalking();
                _finalAreaSequence.MoveCamera();
            }

            else
            {
                _textModifier.UpdateTextTrio("Forgetting... something...", Color.cyan, FontStyles.Normal);
                _textModifier.Fade(true, 10);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InvisiblePlayer"))
        {
            _textModifier.Fade(false, 10);
        }
    }
}
