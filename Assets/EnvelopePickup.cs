using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EnvelopePickup : MonoBehaviour
{
    private bool _isPlayerPresent;

    private TextModifier _textModifier;

    private OrbManager _orbManager;

    public bool _isNotGramBallot;

    public string label;
    public string actionDescription;

    public UnityEvent OnPickUpBallot ;

    public bool HasPickedUp;

    public MeshRenderer[] MeshRenderers;


    // Start is called before the first frame update
    void Awake()
    {
        MeshRenderers = GetComponentsInChildren<MeshRenderer>();
        _textModifier = SingletonManager.Get<TextModifier>();
        _orbManager = SingletonManager.Get<OrbManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HasPickedUp)
            return;

        if(_isPlayerPresent && Input.GetButtonDown("Action"))
            Pickup();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HasPickedUp)
            return;

        if (other.CompareTag("Player"))
        {
            _isPlayerPresent = true;
            _orbManager.SetCanAttack(false);

            if(!_isNotGramBallot)
              _textModifier.UpdateTextTrio("Gram Gram's Ballot", Color.white, FontStyles.Normal);

            else
            {
                _textModifier.UpdateTextTrio(label, Color.white, FontStyles.Normal);
            }
            _textModifier.Fade(true, 10);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (HasPickedUp)
            return;

        if (other.CompareTag("Player"))
        {
            _isPlayerPresent = false;
            _orbManager.SetCanAttack(true);
            _textModifier.Fade(false, 10);
        }
    }

    private void Pickup()
    {
        _isPlayerPresent = false;
        _orbManager.SetCanAttack(true);

        if(!_isNotGramBallot)
            _textModifier.UpdateText("Picked up Gram Gram's ballot.");
        else
        {
            _textModifier.UpdateText(actionDescription);
        }
        _textModifier.AutoTimeFades();

        if(!_isNotGramBallot)
          SingletonManager.Get<OrbManager>().UnlockOrb();

        HasPickedUp = true;
        foreach (MeshRenderer meshRenderer in MeshRenderers)
        {
            meshRenderer.enabled = false;
        }

        OnPickUpBallot.Invoke();
    }

    public void Reset()
    {
        HasPickedUp = false;

        foreach (MeshRenderer meshRenderer in MeshRenderers)
        {
            meshRenderer.enabled = true;
        }
    }
}
