using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

public class GrandmaHouseSequence : MonoBehaviour
{
    [SerializeField] private TextModifier _textModifier;
    [SerializeField] private ScreenFader _screenFader;
    [SerializeField] private Animator _grandmaAnim;
    [SerializeField] private Animator _playerAnim;
    [SerializeField] private GameObject _playerGameObject;
    [SerializeField] private GameObject _dummyPlayer;
    [SerializeField] private Camera[] _cameras;
    [SerializeField] private float[] _cameraFOVs;
    [SerializeField] private GameObject _ballotGameObject;
    [SerializeField] private GameObject _dummyEnvelope;
    [SerializeField] private GameObject _actualEnvelope;
    [SerializeField] private Vector3 _deadGrannyCamAngle;
    [SerializeField] private float _deadGrannyFOV;
    [SerializeField] private float _ballotFOV;
    [SerializeField] private Vector3 _ballotCamAngle;
    [SerializeField] private GameObject _ghostToRaise;
    [SerializeField] private GameObject _playerPaper;
    [SerializeField] private float _startingFOV;

    private bool _hasPlayerHitSpace;
    private bool _resumeSequence;

    [SerializeField] private string[] _dialogueStrings;
    [SerializeField] private Color[] _dialogueColors;
    [SerializeField] private FontStyles[] _dialogueFontStyles;

    [SerializeField] private bool _capFPS;



    // Start is called before the first frame update
    void Start()
    {
        if (_capFPS)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
        }
      
        Cursor.visible = false;
        _startingFOV = _cameras[0].fieldOfView;
        StartCoroutine(Sequence());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Action"))
            _hasPlayerHitSpace = true;
    }

    public void ResumeSequence()
    {
        _resumeSequence = true;
    }

    IEnumerator ZoomCamera(float fOV, float zoomTime)
    {
        float timePassed = 0;
        float startingFOV = _cameras[0].fieldOfView;

        while (timePassed < zoomTime)
        {
            timePassed += Time.deltaTime;
            foreach (Camera camera1 in _cameras)
            {
                camera1.fieldOfView = Mathf.Lerp(startingFOV, fOV, timePassed / zoomTime);
            }
           
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    IEnumerator Sequence()
    {
        _textModifier.UpdateTextTrio(_dialogueStrings[0], _dialogueColors[0], _dialogueFontStyles[0]);
        _textModifier.Fade();
        _hasPlayerHitSpace = false;
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_dialogueStrings[1], _dialogueColors[1], _dialogueFontStyles[1]);
        _textModifier.Fade();
        _hasPlayerHitSpace = false;
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_dialogueStrings[2], _dialogueColors[2], _dialogueFontStyles[2]);
        _textModifier.Fade();
        _screenFader.Fade();
        _hasPlayerHitSpace = false;
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_dialogueStrings[3], _dialogueColors[3], _dialogueFontStyles[3]);
        _textModifier.Fade();
        _hasPlayerHitSpace = false;
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        Coroutine zoomCoroutine = StartCoroutine(ZoomCamera(65, 20f));

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_dialogueStrings[4], _dialogueColors[4], _dialogueFontStyles[4]);
        _textModifier.Fade();
        _hasPlayerHitSpace = false;
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_dialogueStrings[5], _dialogueColors[5], _dialogueFontStyles[5]);
        _textModifier.Fade();
        _hasPlayerHitSpace = false;
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_dialogueStrings[6], _dialogueColors[6], _dialogueFontStyles[6]);
        _textModifier.Fade();
        _hasPlayerHitSpace = false;
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_dialogueStrings[7], _dialogueColors[7], _dialogueFontStyles[7]);
        _textModifier.Fade();
        _hasPlayerHitSpace = false;
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_dialogueStrings[8], _dialogueColors[8], _dialogueFontStyles[8]);
        _textModifier.Fade();
        _hasPlayerHitSpace = false;
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_dialogueStrings[9], _dialogueColors[9], _dialogueFontStyles[9]);
        _textModifier.Fade();
        _hasPlayerHitSpace = false;

        _grandmaAnim.Play("GrandmaCough");

        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        StopCoroutine(zoomCoroutine);

        foreach (Camera camera1 in _cameras)
        {
            camera1.fieldOfView = 35f;
        }
        

        _grandmaAnim.Play("GrandmaSit");
        _playerAnim.Play("PlayerConcerned");

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_dialogueStrings[10], _dialogueColors[10], _dialogueFontStyles[10]);
        _textModifier.Fade();
        _hasPlayerHitSpace = false;
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_dialogueStrings[11], _dialogueColors[11], _dialogueFontStyles[11]);
        _textModifier.Fade();
        _hasPlayerHitSpace = false;
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }


        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_dialogueStrings[12], _dialogueColors[12], _dialogueFontStyles[12]);
        _textModifier.Fade();
        _hasPlayerHitSpace = false;
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(_dialogueStrings[13], _dialogueColors[13], _dialogueFontStyles[13]);
        _textModifier.Fade();
        _hasPlayerHitSpace = false;
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        _textModifier.Fade(false, 1);
        _screenFader.Fade(1, false);
        yield return new WaitForSeconds(1f);
        SingletonManager.Get<MusicManager>().PlayTrack(1);
        _ballotGameObject.SetActive(true);

        while (!_resumeSequence)
        {
            yield return new WaitForEndOfFrame();
        }

        _resumeSequence = false;

        _grandmaAnim.Play("GrandmaDead");
        _dummyEnvelope.SetActive(false);
        _actualEnvelope.SetActive(true);
        SingletonManager.Get<MusicManager>().StopPlaying();
        _ballotGameObject.SetActive(false);
        _screenFader.Fade();

        _textModifier.UpdateTextTrio(_dialogueStrings[14], _dialogueColors[14], _dialogueFontStyles[14]);
        _textModifier.AutoTimeFades(2);
        
        float timePassed = 0;
        float zoomTime = 10;
        float startingFOV = _cameras[0].fieldOfView;
        float nextTime = 3f;
        int timesHit = 0;

        while (timePassed < zoomTime)
        {
            timePassed += Time.deltaTime;

            foreach (Camera camera1 in _cameras)
            {
                camera1.fieldOfView = Mathf.Lerp(startingFOV, _deadGrannyFOV, timePassed / zoomTime);
                camera1.transform.eulerAngles = Vector3.Lerp(Vector3.zero, _deadGrannyCamAngle, timePassed / zoomTime);
            }

            if (timePassed > nextTime)
            {
                if (timesHit == 0)
                {
                    _textModifier.UpdateTextTrio(_dialogueStrings[15], _dialogueColors[15], _dialogueFontStyles[15]);
                    _textModifier.AutoTimeFades(2);
                    nextTime = 6;
                    timesHit++;

                }

                else if (timesHit == 1)
                {
                    _textModifier.UpdateTextTrio(_dialogueStrings[16], _dialogueColors[16], _dialogueFontStyles[16]);
                    _textModifier.AutoTimeFades(2);
                    nextTime = zoomTime + 1;
                    timesHit++;

                    //SingletonManager.Get<MusicManager>().PlayTrack(0);
                }

              
            }
            yield return new WaitForEndOfFrame();
        }

        _ghostToRaise.GetComponent<GhostRaiser>().RaiseGhost();
        

         timePassed = 0;
         zoomTime = 5;

         while (timePassed < 10)
         {
             timePassed += Time.deltaTime;
             yield return new WaitForEndOfFrame();
         }

         timePassed = 0;


        while (timePassed < zoomTime)
        {
            timePassed += Time.deltaTime;

            foreach (Camera camera1 in _cameras)
            {
                camera1.fieldOfView = Mathf.Lerp(_deadGrannyFOV, _ballotFOV, timePassed / zoomTime);
                camera1.transform.eulerAngles = Vector3.Lerp(_deadGrannyCamAngle, _ballotCamAngle, timePassed / zoomTime);
            }
            
            yield return new WaitForEndOfFrame();
        }

        timePassed = 0;

        while (timePassed < 3)
        {
            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        foreach (Camera camera1 in _cameras)
        {
            camera1.fieldOfView = _startingFOV;
            camera1.transform.eulerAngles = Vector3.zero;
        }
      
        _dummyPlayer.SetActive(false);
        _playerPaper.SetActive(true);
        _playerGameObject.GetComponent<PlayerController>().ToggleIsActive();
        //TODO: return camera to normal, enable player, disable player stand in!



        yield return null;
    }
}
