using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

public class GrannyDoor : MonoBehaviour
{
    [SerializeField] private bool _isOpen;

    [SerializeField] private bool _isPlayerPresent;

    [SerializeField] private CheckPoint _firstCheckPoint;

    private TextModifier _textModifier;
    private OrbManager _orbManager;

    

    // Start is called before the first frame update
    void Awake()
    {
        _textModifier = SingletonManager.Get<TextModifier>();
        _orbManager = SingletonManager.Get<OrbManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_isPlayerPresent && Input.GetButtonDown("Action"))
            OpenDoor();
    }

    void OpenDoor()
    {
        if (_isOpen)
        {
            _orbManager.SetCanAttack(true);
            SingletonManager.Get<MusicManager>().PlayTrack(0);
            _textModifier.UpdateTextTrio("Heading outside", Color.white, FontStyles.Normal);
            _textModifier.AutoTimeFades();
            _firstCheckPoint.InitializeCheckpoint();
        }

        else
        {
            _textModifier.UpdateTextTrio("Door blocked by ghost...", Color.white, FontStyles.Normal);
        }
    }

    public void SetDoorOpen()
    {
        _isOpen = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerPresent = true;
            _textModifier.UpdateTextTrio("Door", Color.white, FontStyles.Normal);
            _textModifier.Fade(true, 10);
            _orbManager.SetCanAttack(false);
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
}
