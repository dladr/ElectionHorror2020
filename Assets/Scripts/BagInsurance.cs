using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

public class BagInsurance : MonoBehaviour
{
    public string Dialogue;
    public string Dialogue2;

    public Color Color;

    public FontStyles FontStyles;

    public bool HasTriggered;

    private TextModifier _textModifier;
    private PlayerController _playerController;
    private OrbManager _orbManager;

    [SerializeField] private GameObject _blockerGameObject;

    private void Awake()
    {
        _textModifier = SingletonManager.Get<TextModifier>();
        _playerController = SingletonManager.Get<PlayerController>();
        _orbManager = SingletonManager.Get<OrbManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!HasTriggered && other.CompareTag("Player"))
        {
            if (_playerController.HasBag)
            {
                HasTriggered = true;
                _orbManager.SetCanAttack(false);
                _textModifier.UpdateTextTrio(Dialogue2, Color, FontStyles);
                _textModifier.AutoTimeFades();
                _blockerGameObject.SetActive(false);
            }

            else
            {
                _textModifier.UpdateTextTrio(Dialogue, Color, FontStyles);
                _textModifier.AutoTimeFades();
            }
        }
    }
}
