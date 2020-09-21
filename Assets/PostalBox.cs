using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using UnityEngine;

public class PostalBox : MonoBehaviour
{
    public bool HasMail = true;

   [SerializeField] private bool CanCollectMail;

    OrbManager _orbManager;
    // Start is called before the first frame update
    void Awake()
    {
        _orbManager = SingletonManager.Get<OrbManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Action") && CanCollectMail)
        {
            CollectMail();
        }
    }

    public void CollectMail()
    {
        if (!HasMail)
            return;

        HasMail = false;
        CanCollectMail = false;
        _orbManager.UnlockOrb();
        _orbManager.SetCanAttack(true);
        _orbManager.StartAttack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && HasMail)
        {
            CanCollectMail = true;
            _orbManager.SetCanAttack(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanCollectMail = false;
            _orbManager.SetCanAttack(true);
        }
    }
}
