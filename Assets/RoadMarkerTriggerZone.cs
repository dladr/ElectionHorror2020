using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

public class RoadMarkerTriggerZone : MonoBehaviour
{
    [SerializeField] bool _isOpen;

    private TextModifier _textModifier;

    public CheckPoint NextCheckPoint;
    // Start is called before the first frame update
    void Awake()
    {
        _textModifier = SingletonManager.Get<TextModifier>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _textModifier.UpdateTextTrio("I think I should get back to the truck...", Color.white, FontStyles.Normal);
            _textModifier.Fade(true, 10f);
        }

        else if(other.CompareTag("Truck"))
        {
            if (!_isOpen)
            {
                _textModifier.UpdateTextTrio("I can't leave any mail behind... ", Color.white, FontStyles.Normal);
                _textModifier.Fade(true, 10f);
            }

            else
            {
                SingletonManager.Get<GameManager>().UpdateLastCheckPoint(NextCheckPoint);
            }
          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Truck"))
        {
            _textModifier.Fade(false, 10f);
        }

       
    }

    public  void Open()
    {
        _isOpen = true;
    }

    public void Close()
    {
        _isOpen = false;
    }
}
