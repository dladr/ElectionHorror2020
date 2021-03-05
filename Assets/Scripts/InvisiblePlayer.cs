using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

public class InvisiblePlayer : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField] private float _speed;

    public bool IsActive;

    [SerializeField] private Ghost _myGhost;

    public bool IsBagPresent;
    public bool HasCollectedBag;

    private Collider _bagCollider;

    private TextModifier _textModifier;

    [SerializeField] private Ghost[] _finalGhosts;
    [SerializeField] private List<Transform> _bagTransforms;

    public bool IsAutoWalking;

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _textModifier = SingletonManager.Get<TextModifier>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsActive)
            return;

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        Move(new Vector2(xInput, yInput).normalized);

        if (!HasCollectedBag && IsBagPresent && Input.GetButtonDown("Action"))
        {
            HasCollectedBag = true;
            _bagTransforms.Remove(_bagCollider.transform.parent.transform);
            _myGhost.PickupBag(_bagCollider);
            IsBagPresent = false;
            _textModifier.UpdateTextTrio("Must... Deliver...", Color.cyan, FontStyles.Normal);
            _textModifier.AutoTimeFades();
            AssignBagsToGhosts();
        }
    }

    void AssignBagsToGhosts()
    {
        for (int i = 0; i < _finalGhosts.Length; i++)
        {
            _finalGhosts[i].SetAlternateDestination(_bagTransforms[i]);
           // _finalGhosts[i].IsLookingAtLookTarget = false;
        }
    }
    void Move(Vector2 inputDirection)
    {
        if(IsAutoWalking)
            inputDirection = Vector2.up;

        _rigidbody.velocity = transform.forward * inputDirection.y * _speed +
                              transform.right * inputDirection.x * _speed;

        
    }


    public void StartAutoWalking()
    {
        IsAutoWalking = true;
        _myGhost.IsLookingUp = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!HasCollectedBag && other.CompareTag("Bag"))
        {
            IsBagPresent = true;
            _bagCollider = other;
            _textModifier.UpdateTextTrio("Mail Bag", Color.cyan, FontStyles.Normal);
            _textModifier.Fade(speed: 10);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsBagPresent && other.CompareTag("Bag"))
        {
            IsBagPresent = false;
            _textModifier.Fade(false, 10);
            _bagCollider = null;
        }
    }

    public void SetActive()
    {
        IsActive = true;
        //_myGhost.transform.localEulerAngles = Vector3.zero;
        _myGhost.IsPlayerGhostLooking = true;
    }

}
