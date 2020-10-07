using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

public class RearDoor : MonoBehaviour
{
    [SerializeField] public bool IsOpen;


    [SerializeField] private bool _isPlayerPresent;

    [SerializeField] private PlayerController _playerController;
    [SerializeField] private OrbManager _orbManager;

    public bool IsMailToCollectNearby;

    private Animator _anim;

    private TextModifier _textModifier;

    // Start is called before the first frame update
    void Awake()
    {
        _anim = GetComponent<Animator>();
        _textModifier = SingletonManager.Get<TextModifier>();
        _playerController = SingletonManager.Get<PlayerController>();
        _orbManager = SingletonManager.Get<OrbManager>();
    }

    private void Start()
    {
        _anim.SetBool("IsOpen", true);
        IsOpen = true;
    }

    void Update()
    {
        if (_isPlayerPresent && Input.GetButtonDown("Action"))
        {
            TakeAction();
        }
    }

    void TakeAction()
    {
        if (!IsOpen)
        {
            if(IsMailToCollectNearby)
                OpenDoor();
            else
            {
                _textModifier.UpdateTextTrio("I don't need a bag right now...", Color.white, FontStyles.Normal);
            }
        }
            

        else if (IsOpen && !_playerController.HasBag && IsMailToCollectNearby)
        {
           _playerController.GetBag();
        }

        else if (IsOpen && _playerController.HasBag)
        {
            if (_playerController.IsBagFull)
            {
                _playerController.DepositBag();
                IsMailToCollectNearby = false;
            }

            else
            {
                _textModifier.UpdateTextTrio("I have to collect the mail first...", Color.white, FontStyles.Normal);
            }
        }

        else if (IsOpen && !_playerController.HasBag && !IsMailToCollectNearby)
        {
            CloseDoor();
        }

        _textModifier.UpdateTextTrio(GetLabel(), Color.white, FontStyles.Normal);
        _textModifier.Fade(true, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerPresent = true;
            _textModifier.UpdateTextTrio(GetLabel(), Color.white, FontStyles.Normal);
            _textModifier.Fade(true, 10);
            _orbManager.SetCanAttack(false);
        }
    }

    string GetLabel()
    {
        if (!IsOpen)
            return "Back Door";
        else if (IsOpen && !_playerController.HasBag)
        {
            return "Mail Bags";
        }

        else
        {
            return "Back of Truck";
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

    void OpenDoor()
    {
        _anim.SetBool("IsOpen", true);
        IsOpen = true;
        _textModifier.UpdateTextTrio(GetLabel(),Color.white, FontStyles.Normal);
    }

    void CloseDoor()
    {
        _anim.SetBool("IsOpen", false);
        IsOpen = false;
        _textModifier.UpdateTextTrio(GetLabel(), Color.white, FontStyles.Normal);
       // _textModifier.AutoTimeFades();
    }
}
