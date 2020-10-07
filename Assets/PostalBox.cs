using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
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
                Debug.Log("cannot collect mail without bag");
            }

            else 
            {
                Debug.Log("No mail to collect");
            }
        }

      
    }

    public void CollectMail()
    {
        if (!HasMail)
            return;

        HasMail = false;
        CanCollectMail = false;
        _roadMarker.OpenRoad();
        _orbManager.UnlockOrb();
        _orbManager.SetCanAttack(true);
        _orbManager.StartAttack();
        _playerController.PickupBag(true);

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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanCollectMail = false;
            _orbManager.SetCanAttack(true);
            IsPlayerPresent = false;
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
