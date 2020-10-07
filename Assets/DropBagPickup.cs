using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using UnityEngine;

public class DropBagPickup : MonoBehaviour
{
    private bool _isPlayerPresent;
    private PlayerController _playerController;
    [SerializeField] private Animator _anim;
    public bool IsFull;

    private TextModifier _textModifier;

    private OrbManager _orbManager;
    // Start is called before the first frame update
    void Start()
    {
        _textModifier = SingletonManager.Get<TextModifier>();
        _playerController = SingletonManager.Get<PlayerController>();
        _orbManager = SingletonManager.Get<OrbManager>();
        _anim.SetBool("IsVisible", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerPresent && Input.GetButtonDown("Action"))
            Pickup();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerPresent = true;
            _textModifier.UpdateText("Mail Bag");
            _textModifier.Fade(true, 10);
            _orbManager.SetCanAttack(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
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

    private void Pickup()
    {
        _textModifier.Fade(false, 10);
        _playerController.PickupBag(IsFull);
    }
}
