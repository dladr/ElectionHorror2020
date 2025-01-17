﻿using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class CheckPoint : MonoBehaviour
{
    public Transform PlayerPosition;

    public int NumberOfOrbs;

    public GameObject PlayerObject;

    private OrbManager _orbManager;

    public Transform TruckPosition;

    public Ghost[] Ghosts;

    public PostalBox[] PostalBoxes;

    public Mimic[] Mimics;

    public DialogueTrigger[] DialogueTriggers;

    public UnityEvent ResetCheckPoint;

    private GameManager _gameManager;

    private ScreenFader _screenFader;

    private TextModifier _textModifier;

    public GameObject[] ObjectsToSetActive;

    public int EmptyBagIndex;

    public int FullBagIndex;

    public bool IsMailToCollect;
    public bool IsMailBagNearby;

    public bool IsTruckDoorOpen;
    public bool InstantFade;

    public GameObject[] ObjectsToSetActiveOnCheckpointSet;
    public GameObject[] ObjectsToDeactivateOnCheckPointSet;

    public bool IsRitualReset;

    // Start is called before the first frame update
    void Awake()
    {
        _screenFader = SingletonManager.Get<ScreenFader>();
        _textModifier = SingletonManager.Get<TextModifier>();
        _gameManager = SingletonManager.Get<GameManager>();
        _orbManager = SingletonManager.Get<OrbManager>();
    }

    private void Start()
    {
       // _screenFader.Fade();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public void Reset()
    {
        if (IsRitualReset)
        {
            StartCoroutine(ResetSequenceRitual());
        }

        else
        {
            StartCoroutine(ResetSequence());
        }
        
    }

    public void InitializeCheckpoint()
    {
        StartCoroutine(InitializeSequence());
    }

    IEnumerator ResetSequenceRitual()
    {
        _textModifier.Islocked = true;
        PlayerObject.GetComponentInChildren<PlayerController>().Deactivate();


        if (!InstantFade)
            _screenFader.Fade(isFadingIn: false);
        else
        {
            _screenFader.Fade(15, false);
        }

        yield return new WaitForSeconds(1f);

        //foreach (GameObject o in ObjectsToSetActive)
        //{
        //    o.SetActive(true);
        //}

        ResetCheckPoint.Invoke();

        PlayerObject.transform.position = PlayerPosition.position;

        //if (!TruckPosition.SafeIsUnityNull())
        //{
        //    TruckMovement truckMovement = SingletonManager.Get<TruckMovement>();
        //    truckMovement.transform.position = TruckPosition.position;
        //    truckMovement.transform.rotation = TruckPosition.rotation;
        //}

        SingletonManager.Get<OrbManager>().SetNumberOfOrbs(NumberOfOrbs);

        //if (Ghosts.IsNullOrEmpty())
        //    Ghosts = GetComponentsInChildren<Ghost>();

        //if (!Ghosts.IsNullOrEmpty())
        //{
        //    foreach (Ghost ghost in Ghosts)
        //    {
        //        ghost.Reset();
        //    }
        //}


        //if (PostalBoxes.IsNullOrEmpty())
        //{
        //    PostalBoxes = GetComponentsInChildren<PostalBox>();
        //}

        //if (!PostalBoxes.IsNullOrEmpty())
        //{
        //    foreach (PostalBox postalBox in PostalBoxes)
        //    {
        //        postalBox.Reset();
        //    }
        //}

        //if (!Mimics.IsNullOrEmpty())
        //{
        //    foreach (Mimic mimic in Mimics)
        //    {
        //        mimic.Reset();
        //    }
        //}


        //if (!DialogueTriggers.IsNullOrEmpty())
        //    foreach (DialogueTrigger dialogueTrigger in DialogueTriggers)
        //    {
        //        dialogueTrigger.Reset();
            //}

        //PlayerObject.GetComponent<PlayerController>().Reset();


        // _textModifier.Fade(false, 10);

        PlayerObject.GetComponentInChildren<PlayerController>().SetActive();
        _orbManager.SetCanAttack(true);
        _screenFader.Fade();

        //SingletonManager.Get<RearDoor>().Reset(IsTruckDoorOpen, EmptyBagIndex, FullBagIndex, IsMailToCollect, IsMailBagNearby);

        if (_textModifier.Islocked)
        {
            _textModifier.Islocked = false;
            // _textModifier.Fade(false, 10);
        }

        yield return null;
    }
    IEnumerator ResetSequence()
    {
        _textModifier.Islocked = true;
        PlayerObject.GetComponentInChildren<PlayerController>().Deactivate();
        

        if(!InstantFade)
            _screenFader.Fade(isFadingIn: false);
        else
        {
            _screenFader.Fade(15, false);
        }
       
        yield return new WaitForSeconds(1f);

        foreach (GameObject o in ObjectsToSetActive)
        {
            o.SetActive(true);
        }

        ResetCheckPoint.Invoke();

        PlayerObject.transform.position = PlayerPosition.position;

        if (!TruckPosition.SafeIsUnityNull())
        {
            TruckMovement truckMovement = SingletonManager.Get<TruckMovement>();
            truckMovement.transform.position = TruckPosition.position;
            truckMovement.transform.rotation = TruckPosition.rotation;
        }

        SingletonManager.Get<OrbManager>().SetNumberOfOrbs(NumberOfOrbs);

        if (Ghosts.IsNullOrEmpty())
            Ghosts = GetComponentsInChildren<Ghost>();

        if (!Ghosts.IsNullOrEmpty())
        {
            foreach (Ghost ghost in Ghosts)
            {
                ghost.Reset();
            }
        }
        

        if (PostalBoxes.IsNullOrEmpty())
        {
            PostalBoxes = GetComponentsInChildren<PostalBox>();
        }

        if (!PostalBoxes.IsNullOrEmpty())
        {
            foreach (PostalBox postalBox in PostalBoxes)
            {
                postalBox.Reset();
            }
        }

        if (!Mimics.IsNullOrEmpty())
        {
            foreach (Mimic mimic in Mimics)
            {
                mimic.Reset();
            }
        }

        
        if(!DialogueTriggers.IsNullOrEmpty())
            foreach (DialogueTrigger dialogueTrigger in DialogueTriggers)
            {
                dialogueTrigger.Reset();
            }

        PlayerObject.GetComponent<PlayerController>().Reset();

        
       // _textModifier.Fade(false, 10);

        PlayerObject.GetComponentInChildren<PlayerController>().SetActive();
        _orbManager.SetCanAttack(true);
        _screenFader.Fade();

        SingletonManager.Get<RearDoor>().Reset(IsTruckDoorOpen, EmptyBagIndex, FullBagIndex, IsMailToCollect, IsMailBagNearby);

        if (_textModifier.Islocked)
        {
            _textModifier.Islocked = false;
           // _textModifier.Fade(false, 10);
        }
        
        yield return null;
    }

    IEnumerator InitializeSequence()
    {
        PlayerObject.GetComponentInChildren<PlayerController>().Deactivate();
        PlayerObject.GetComponent<PlayerController>()._speed = .75f;

        _screenFader.Fade(isFadingIn: false);
        yield return new WaitForSeconds(1f);

        foreach (GameObject o in ObjectsToSetActive)
        {
            o.SetActive(true);
        }

        ResetCheckPoint.Invoke();

        PlayerObject.transform.position = PlayerPosition.position;
        if (!TruckPosition.SafeIsUnityNull())
        {
            SingletonManager.Get<TruckMovement>().transform.position = TruckPosition.position;
            SingletonManager.Get<TruckMovement>().transform.rotation = TruckPosition.rotation;
        }
            

      
        _textModifier.Fade(false, 10);

        PlayerObject.GetComponentInChildren<PlayerController>().SetActive();
        _screenFader.Fade();

        yield return null;
    }
    public void SetCheckPoint()
    {
        _gameManager.UpdateLastCheckPoint(this);

        foreach (GameObject o in ObjectsToSetActiveOnCheckpointSet)
        {
            o.SetActive(true);
        }

        foreach (GameObject o in ObjectsToDeactivateOnCheckPointSet)
        {
            o.SetActive(false);
        }
    }
}
