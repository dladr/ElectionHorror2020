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

    [SerializeField] bool _isVisible;
    [SerializeField] private bool _canSetVisible;

    private TextModifier _textModifier;

    private OrbManager _orbManager;

    [SerializeField] private Rigidbody _rigidbody;

    public Vector3 InitialPosition;

    public Quaternion InitialRotation;
    // Start is called before the first frame update
    void Awake()
    {
        _textModifier = SingletonManager.Get<TextModifier>();
        _playerController = SingletonManager.Get<PlayerController>();
        _orbManager = SingletonManager.Get<OrbManager>();
        _anim.SetBool("IsVisible", true);
        _isVisible = true;

        InitialPosition = transform.parent.position;
        InitialRotation = transform.parent.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerPresent && Input.GetButtonDown("Action"))
            Pickup();

        if (!_canSetVisible)
        {
            _anim.SetBool("IsVisible", true);
        }
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
        if (!_orbManager.CheckInternalCanAttack())
            return;

        _rigidbody.isKinematic = false;
        _textModifier.Fade(false, 10);
        _orbManager.SetCanAttack(true);
        _playerController.PickupBag(IsFull);
    }

    public void Reset()
    {
        transform.parent.position = InitialPosition;
        transform.parent.rotation = InitialRotation;
        _anim.SetBool("IsVisible", true);
        IsFull = false;
        _anim.SetBool("IsFull", false);
        _isVisible = true;
        _canSetVisible = false;
    }

    public void SetCanSetVisible(bool canSet)
    {
        _canSetVisible = canSet;
    }

    public void SetIsBagFull(bool isFull)
    {
        _anim.SetBool("IsFull", isFull);
    }

    public void SetVisible(bool isVisible)
    {
        if (!_canSetVisible)
            return;

        _isVisible = true;
        _anim.SetBool("IsVisible", isVisible);
    }
}
