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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Action"))
            _hasPlayerHitSpace = true;
    }

    IEnumerator Sequence()
    {



        yield return null;
    }
}
