using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrandmaHouseSequence : MonoBehaviour
{
    [SerializeField] private TextModifier _textModifier;
    [SerializeField] private ScreenFader _screenFader;

    private bool _hasPlayerHitSpace;

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

        Debug.Log("Granny started coughing");
        while (!_hasPlayerHitSpace)
        {
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Granny stopped coughing");

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

        Debug.Log("Time To Vote");

        yield return null;
    }
}
