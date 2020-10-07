using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

public class PostalBox : MonoBehaviour
{
    public bool HasMail = true;

   [SerializeField] private bool CanCollectMail;

   public bool IsPlayerPresent;

    OrbManager _orbManager;
    private PlayerController _playerController;

    [SerializeField] private RoadMarker _roadMarker;

    [SerializeField] private MaterialSetter[] _materialSetters;

    private TextModifier _textModifier;
    // Start is called before the first frame update
    void Awake()
    {
        _orbManager = SingletonManager.Get<OrbManager>();
        _playerController = SingletonManager.Get<PlayerController>();
        _textModifier = SingletonManager.Get<TextModifier>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Action") && IsPlayerPresent)
        {
            if(CanCollectMail)
              CollectMail();

            else if(HasMail)
            {
                _textModifier.UpdateText("I need a mail bag...");
            }

            else 
            {
                _textModifier.UpdateText("Empty");
            }
        }

      
    }

    public void CollectMail()
    {
        if (!HasMail)
            return;

        HasMail = false;
        CanCollectMail = false;
        _playerController.IsBagFull = true;
        _roadMarker.OpenRoad();
        _orbManager.UnlockOrb();
        _orbManager.SetCanAttack(true);
        _orbManager.StartAttack();
        _textModifier.UpdateText("Mail collected");

        foreach (MaterialSetter materialSetter in _materialSetters)
        {
            materialSetter.TurnOffGlow();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (_playerController.HasBag && HasMail)
            {
                CanCollectMail = true;
            }

            IsPlayerPresent = true;
            _orbManager.SetCanAttack(false);
            _textModifier.UpdateTextTrio("Collection Box", Color.blue, FontStyles.Normal);
            _textModifier.Fade(true, 10f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanCollectMail = false;
            _orbManager.SetCanAttack(true);
            IsPlayerPresent = false;
            _textModifier.Fade(false, 10f);
        }
    }

    public void Reset()
    {
        if (!HasMail)
        {
            _roadMarker.CloseRoad();
            HasMail = true;

            foreach (MaterialSetter materialSetter in _materialSetters)
            {
                materialSetter.TurnOnGlow();
            }
        }
        
    }
}
