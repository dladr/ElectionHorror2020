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

        yield return null;
    }
}
