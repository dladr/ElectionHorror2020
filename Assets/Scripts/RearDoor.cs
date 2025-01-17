﻿using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class RearDoor : MonoBehaviour
{
    [SerializeField] public bool IsOpen;
    public bool IsInteractable;

    [SerializeField] private bool _isPlayerPresent;

    [SerializeField] private PlayerController _playerController;
    [SerializeField] private OrbManager _orbManager;

    public bool IsMailToCollectNearby;
    public bool IsMailBagNearby;

    private Animator _anim;

    private TextModifier _textModifier;

    [SerializeField] private GameObject[] _emptyBags;
    [SerializeField] private GameObject[] _fullBags;

    private int _emptyBagIndex;
    private int _fullBagIndex;

    [SerializeField] private GameObject _strangerGameObject;
    [SerializeField] private GameObject _strangerCorpseGameObject;
    [SerializeField] private Animator _strangerAnim;

    public bool IsDrivingWithBackOpen;

    [SerializeField] private String[] _strangerPickupStrings;
    [SerializeField] private String[] _strangerDropOffStrings;
    [SerializeField] private Color _strangerDialogueColor;
    [SerializeField] private FontStyles[] _strangerPickupFontStyles;
    [SerializeField] private FontStyles[] _strangerDropOffFontStyles;

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
        //_anim.SetBool("IsOpen", true);
        //IsOpen = true;
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
        if (!IsInteractable)
            return;

        if (!IsOpen)
        {
            if (IsMailToCollectNearby)
            {
                OpenDoor();
                _textModifier.UpdateTextTrio(GetLabel(), Color.white, FontStyles.Normal);

            }
            
            else
            {
                if (IsDrivingWithBackOpen)
                {
                    _textModifier.UpdateTextTrio("Maybe I should keep it open...", Color.white, FontStyles.Normal);
                    OpenDoor();
                }

                else
                {
                    _textModifier.UpdateTextTrio("I don't need a bag right now...", Color.white, FontStyles.Normal);
                }
                
            }
        }
            

        else if (IsOpen && !_playerController.HasBag && IsMailToCollectNearby)
        {
            if (IsMailBagNearby)
            {
                _textModifier.UpdateTextTrio("I don't need ANOTHER bag now...", Color.white, FontStyles.Normal);
            }

            else
            {
                _playerController.GetBag();
                IsMailBagNearby = true;
                RemoveEmptyBag();
                AttemptPickupLine();
                _textModifier.UpdateTextTrio(GetLabel(), Color.white, FontStyles.Normal);
            }
           
        }

        else if (IsOpen && _playerController.HasBag)
        {
            if (_playerController.IsBagFull)
            {
                _playerController.DepositBag();
                AddFullBag();
                AttemptDropOffLine();
                IsMailToCollectNearby = false;
                IsMailBagNearby = false;
                _textModifier.UpdateTextTrio(GetLabel(), Color.white, FontStyles.Normal);
            }

            else
            {
                _textModifier.UpdateTextTrio("I have to collect the mail first...", Color.white, FontStyles.Normal);
            }
        }

        else if (IsOpen && !_playerController.HasBag && !IsMailToCollectNearby)
        {
            if (IsDrivingWithBackOpen)
            {
                _textModifier.UpdateTextTrio("Maybe I should leave it open...", Color.white, FontStyles.Normal);
            }

            else
            {
                CloseDoor();
                _textModifier.UpdateTextTrio(GetLabel(), Color.white, FontStyles.Normal);
            }
          
        }

        _textModifier.Fade(true, 10);
    }

    void AttemptPickupLine()
    {
        if (_emptyBagIndex > 1 && _emptyBagIndex < 6)
        {
            int pickupLineIndex = _emptyBagIndex - 2;
            _textModifier.DisplayImmutableMessage(_strangerPickupStrings[pickupLineIndex], _strangerDialogueColor, _strangerPickupFontStyles[pickupLineIndex]);
        }
    }

    void AttemptDropOffLine()
    {
        if (_fullBagIndex > 2 && _fullBagIndex < 6)
        {
            int dropOffIndex = _fullBagIndex - 3;
            _textModifier.DisplayImmutableMessage(_strangerDropOffStrings[dropOffIndex], _strangerDialogueColor, _strangerDropOffFontStyles[dropOffIndex]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_playerController.IsActive)
                return;

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
        else if (IsOpen && !_playerController.HasBag && IsMailToCollectNearby)
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
            if (!_playerController.IsActive)
                return;

            _isPlayerPresent = false;
            _textModifier.Fade(false, 10);
            _orbManager.SetCanAttack(true);
        }
    }

    void RemoveEmptyBag()
    {
        _emptyBags[_emptyBagIndex].SetActive(false);
        _emptyBagIndex++;
    }

    void SetEmptyBagIndex(int index)
    {
        if (_emptyBagIndex == index)
            return;

        _emptyBagIndex = index;

        foreach (GameObject emptyBag in _emptyBags)
        {
            emptyBag.SetActive(true);
        }

        if (_emptyBagIndex > 0)
            for (int i = 0; i < _emptyBagIndex; i++)
            {
                _emptyBags[i].SetActive(false);
            }
    }

    void AddFullBag()
    {
        _fullBags[_fullBagIndex].SetActive(true);
        _fullBagIndex++;
    }

    void SetFullBagIndex(int index)
    {
        if (_fullBagIndex == index)
            return;

        _fullBagIndex = index;

        foreach (GameObject fullBag in _fullBags)
        {
            fullBag.SetActive(false);
        }

        if (_fullBagIndex > 0)
            for (int i = 0; i < _fullBagIndex; i++)
            {
                _fullBags[i].SetActive(true);
            }
    }
 public void OpenDoor()
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

    [Button]
    public void Reset(bool isTruckDoorOpen, int emptyBagIndex, int fullBagIndex, bool isMailToCollect, bool isBagNearby = false)
    {
        if(isTruckDoorOpen)
            OpenDoor();
        else
        {
            CloseDoor();
        }

        SetEmptyBagIndex(emptyBagIndex);
        SetFullBagIndex(fullBagIndex);
        IsMailToCollectNearby = isMailToCollect;
        IsMailBagNearby = isBagNearby;

    }

    public void ActivateStranger(bool isActivating)
    {
        _strangerGameObject.SetActive(isActivating);
    }

    [Button]
    public void KillStranger()
    {
        _strangerGameObject.SetActive(false);
        _strangerCorpseGameObject.SetActive(true);
        IsDrivingWithBackOpen = true;
        SingletonManager.Get<TruckMovement>().IsDrivingWithDoorOpen = true;
    }
}
