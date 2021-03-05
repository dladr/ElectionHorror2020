using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class FinalAreaSequence : MonoBehaviour
{
    private ScreenFader _screenFader;
    [SerializeField] private InvisiblePlayer _invisiblePlayer;
    [SerializeField] private GhostRaiserAnimStyle _ghostRaiser;
    [SerializeField] private Ghost[] _ghosts;
    [SerializeField] private Camera _camera;
    [SerializeField] private Camera _gramCam;
    [SerializeField] private Transform _playerChildren;
    [SerializeField] private Transform _ghostsTransform;
    [SerializeField] private float _lowerGhostFloat;
    [SerializeField] private Transform _cameraStartTransform;
    [SerializeField] private Transform _cameraEndTransform;
    [SerializeField] private GameObject _gramHouseGameObject;
    [SerializeField] private GameObject _creditsGameObject;
    [SerializeField] private Vector3 _finalCameraPosition;

    private TextModifier _textModifier;
    private GameManager _gameManager;

    public String[] EndingStrings;
    public Color DialogueColor;
    public FontStyles FontStyles;
    public bool IsStartingOnAwake;
   // [SerializeField] private bool _hasPlayerHitSpace;

    [SerializeField] private float _advanceDialogueTime;


    private void Awake()
    {
        _screenFader = SingletonManager.Get<ScreenFader>();
        _textModifier = SingletonManager.Get<TextModifier>();
        _gameManager = SingletonManager.Get<GameManager>();
    }

    private void Start()
    {
        if(IsStartingOnAwake)
           StartSequence();
    }

    private void Update()
    {
        //if (Input.GetButtonDown("Action"))
        //{
        //    _hasPlayerHitSpace = true;
        //}
    }

    [Button]
    public void StartSequence()
    {
        StartCoroutine(Sequence());
    }

    [Button]
    public void MoveCamera()
    {
        StartCoroutine(CameraSequence());
    }

    IEnumerator Sequence()
    {
        RenderSettings.fogDensity = 0;
        _screenFader.Fade(.5f);
        SingletonManager.Get<MusicManager>().PlayTrack(2, isLooping:false);

        yield return new WaitForSeconds(5);

        yield return new WaitForSeconds(2);

        foreach (Ghost ghost in _ghosts)
        {
            ghost.ToggleIsActive();
        }

        yield return  new WaitForSeconds(10);

        _ghostRaiser.RaiseGhost();

        yield return  new WaitForSeconds(7);

        _invisiblePlayer.SetActive();

        yield return null;
    }

    private IEnumerator CameraSequence()
    {
        float transitionTime = 10;
        float timeElapsed = 0;
        float originalGhostsY = _ghostsTransform.localPosition.y;
        float originalPlayerChildrenY = _playerChildren.localPosition.y;
        Vector3 originalGhostsPosition = _ghostsTransform.localPosition;
        Vector3 originalPlayerChildrenPosition = _playerChildren.localPosition;

        while (timeElapsed < transitionTime)
        {
            timeElapsed += Time.deltaTime;
            float lerpPercent = timeElapsed / transitionTime;
            yield return new WaitForEndOfFrame();
            _camera.transform.position =
                Vector3.Lerp(_cameraStartTransform.position, _cameraEndTransform.position, lerpPercent);
            _camera.transform.rotation =
                Quaternion.Lerp(_cameraStartTransform.rotation, _cameraEndTransform.rotation, lerpPercent);
            _playerChildren.transform.localPosition =originalPlayerChildrenPosition +  
                                                     Vector3.up * (Mathf.Lerp(originalPlayerChildrenY, (originalPlayerChildrenY - _lowerGhostFloat), lerpPercent) - originalPlayerChildrenY);
            _ghostsTransform.transform.localPosition = originalGhostsPosition +
                                                      Vector3.up * (Mathf.Lerp(originalGhostsY, (originalGhostsY - _lowerGhostFloat), lerpPercent) - originalGhostsY);
        }

        yield return new WaitForSeconds(3);

        _screenFader.Fade(.1f, false);

        yield return  new WaitForSeconds(10);

        _gramHouseGameObject.SetActive(true);

        //Start ending text

        _textModifier.UpdateTextTrio(EndingStrings[0], DialogueColor, FontStyles);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(EndingStrings[1], DialogueColor, FontStyles);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(EndingStrings[9], DialogueColor, FontStyles);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _screenFader.Fade();
        Coroutine zoomCoroutine = StartCoroutine(ZoomCamera(40, 60f));

        _textModifier.UpdateTextTrio(EndingStrings[2], DialogueColor, FontStyles);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_gameManager.GetResultsString(1), DialogueColor, FontStyles);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(EndingStrings[3], DialogueColor, FontStyles);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_gameManager.GetResultsString(3), DialogueColor, FontStyles);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(EndingStrings[4], DialogueColor, FontStyles);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_gameManager.GetResultsString(2), DialogueColor, FontStyles);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        yield return null;

        _textModifier.UpdateTextTrio(EndingStrings[5], DialogueColor, FontStyles);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_gameManager.GetResultsString(4), DialogueColor, FontStyles);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(EndingStrings[6], DialogueColor, FontStyles);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_gameManager.GetResultsString(0), DialogueColor, FontStyles);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        string nextTextToUse = EndingStrings[7];

        if (!_gameManager.BallotAnswers[0])
        {
            nextTextToUse = EndingStrings[8];
        }


        _textModifier.UpdateTextTrio(nextTextToUse, DialogueColor, FontStyles);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _screenFader.Fade(isFadingIn:false);

        yield return new WaitForSeconds(2);

        _creditsGameObject.SetActive(true);

    }

    IEnumerator ZoomCamera(float fOV, float zoomTime)
    {
        float timePassed = 0;
        float startingFOV = _gramCam.fieldOfView;
        Vector3 cameraStartingPosition = _gramCam.transform.localPosition;

        while (timePassed < zoomTime)
        {
            timePassed += Time.deltaTime;
            
                _gramCam.fieldOfView = Mathf.Lerp(startingFOV, fOV, timePassed / zoomTime);
                _gramCam.transform.localPosition =
                    Vector3.Lerp(cameraStartingPosition, _finalCameraPosition, timePassed / zoomTime);

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}
