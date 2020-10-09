using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class CheckPoint : MonoBehaviour
{
    public Transform PlayerPosition;

    public int NumberOfOrbs;

    public GameObject PlayerObject;

    public Transform TruckPosition;

    public Ghost[] Ghosts;

    public PostalBox[] PostalBoxes;

    public UnityEvent ResetCheckPoint;

    private GameManager _gameManager;

    private ScreenFader _screenFader;

    private TextModifier _textModifier;

    public GameObject[] ObjectsToSetActive;

    public int EmptyBagIndex;

    public int FullBagIndex;

    public bool IsMailToCollect;

    public bool IsTruckDoorOpen;

    // Start is called before the first frame update
    void Awake()
    {
        _screenFader = SingletonManager.Get<ScreenFader>();
        _textModifier = SingletonManager.Get<TextModifier>();
        _gameManager = SingletonManager.Get<GameManager>();
    }

    private void Start()
    {
       // _screenFader.Fade();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        StartCoroutine(ResetSequence());
    }

    public void InitializeCheckpoint()
    {
        StartCoroutine(InitializeSequence());
    }

    IEnumerator ResetSequence()
    {
        PlayerObject.GetComponentInChildren<PlayerController>().Deactivate();

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

        SingletonManager.Get<OrbManager>().SetNumberOfOrbs(NumberOfOrbs);

        if (Ghosts.IsNullOrEmpty())
            Ghosts = GetComponentsInChildren<Ghost>();

        foreach (Ghost ghost in Ghosts)
        {
            ghost.Reset();
        }

        if (PostalBoxes.IsNullOrEmpty())
        {
            PostalBoxes = GetComponentsInChildren<PostalBox>();
        }

        foreach (PostalBox postalBox in PostalBoxes)
        {
            postalBox.Reset();
        }

        PlayerObject.GetComponent<PlayerController>().Reset();

        _textModifier.Fade(false, 10);

        PlayerObject.GetComponentInChildren<PlayerController>().SetActive();
        _screenFader.Fade();

        SingletonManager.Get<RearDoor>().Reset(IsTruckDoorOpen, EmptyBagIndex, FullBagIndex, IsMailToCollect);

        yield return null;
    }

    IEnumerator InitializeSequence()
    {
        PlayerObject.GetComponentInChildren<PlayerController>().Deactivate();

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
    }
}
