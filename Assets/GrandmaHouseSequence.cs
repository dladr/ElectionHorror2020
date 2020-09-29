using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Camera _camera;
    [SerializeField] private float[] _cameraFOVs;
    [SerializeField] private GameObject _ballotGameObject;
    [SerializeField] private GameObject _dummyEnvelope;
    [SerializeField] private GameObject _actualEnvelope;

    private bool _hasPlayerHitSpace;
    private bool _resumeSequence;

    [SerializeField] private string[] _dialogueStrings;
    [SerializeField] private Color[] _dialogueColors;
    [SerializeField] private FontStyles[] _dialogueFontStyles;



    // Start is called before the first frame update
    void Start()
    {
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
        float startingFOV = _camera.fieldOfView;

        while (timePassed < zoomTime)
        {
            timePassed += Time.deltaTime;
            _camera.fieldOfView = Mathf.Lerp(startingFOV, fOV, timePassed / zoomTime);
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
        _camera.fieldOfView = 35f;

        _grandmaAnim.Play("GrandmaSit");

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
        _ballotGameObject.SetActive(true);

        while (!_resumeSequence)
        {
            yield return new WaitForEndOfFrame();
        }

        _resumeSequence = false;

        _grandmaAnim.Play("GrandmaDead");
        _dummyEnvelope.SetActive(false);
        _actualEnvelope.SetActive(true);
        _ballotGameObject.SetActive(false);
        _screenFader.Fade();

        yield return null;
    }
}
