using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

public class CrashedCar : MonoBehaviour
{
    public string Title;

    public string[] DialogueStrings;

    public Color[] TextColors;

    public FontStyles[] FontStyles;

    private TextModifier _textModifier;

    private OrbManager _orbManager;

    private bool _isPlayerPresent;

    private ScreenFader _screenFader;

    public Transform _playerLocation;
    public Transform _truckLocation;
    public GameObject TruckGameObject;

    private RearDoor _rearDoor;

  // [SerializeField] private bool _hasPlayerHitSpace;

   private bool _hasStartedSequence;

    private PlayerController _playerController;

    public GameObject TreeGameObject;
    public GameObject TreeMovedGameObject;

    [SerializeField] private float _advanceDialogueTime;



    // Start is called before the first frame update
    void Awake()
    {
        _textModifier = SingletonManager.Get<TextModifier>();
        _orbManager = SingletonManager.Get<OrbManager>();
        _screenFader = SingletonManager.Get<ScreenFader>();
        _rearDoor = SingletonManager.Get<RearDoor>();
        _playerController = SingletonManager.Get<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Action"))
        //    _hasPlayerHitSpace = true;

        if (_isPlayerPresent && Input.GetButtonDown("Action") && !_hasStartedSequence)
        {
            _hasStartedSequence = true;
            StartCoroutine(CrashSequence());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerPresent = true;
            _textModifier.UpdateTextTrio(Title, TextColors[0], FontStyles[0]);
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

    IEnumerator CrashSequence()
    {
        if (_rearDoor.SafeIsUnityNull())
            _rearDoor = SingletonManager.Get<RearDoor>();

        _playerController.Deactivate();

        _textModifier.UpdateTextTrio(DialogueStrings[0], TextColors[0], FontStyles[0]);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(DialogueStrings[1], TextColors[1], FontStyles[1]);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(DialogueStrings[2], TextColors[2], FontStyles[2]);
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

        yield return new WaitForSeconds(.5f);
        //TODO: Play SFX

        TreeGameObject.SetActive(false);
        TreeMovedGameObject.SetActive(true);
        TruckGameObject.transform.position = _truckLocation.position;
        TruckGameObject.transform.rotation = _truckLocation.rotation;
        _playerController.transform.position = _playerLocation.position;
        _playerController.ResetRotation();
        _playerController.ResetAnim();
        _rearDoor.ActivateStranger(true);
        _rearDoor.OpenDoor();
        _rearDoor.IsInteractable = false;


        _screenFader.Fade();

        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(DialogueStrings[3], TextColors[3], FontStyles[3]);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(2);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);


        _textModifier.UpdateTextTrio(DialogueStrings[4], TextColors[4], FontStyles[4]);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(DialogueStrings[5], TextColors[5], FontStyles[5]);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(DialogueStrings[6], TextColors[6], FontStyles[6]);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(2);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(DialogueStrings[7], TextColors[7], FontStyles[7]);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);
        yield return new WaitForSeconds(.5f);

        _textModifier.UpdateTextTrio(DialogueStrings[8], TextColors[8], FontStyles[8]);
        _textModifier.Fade();
        //_hasPlayerHitSpace = false;
        //while (!_hasPlayerHitSpace)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        yield return new WaitForSeconds(_advanceDialogueTime);

        _textModifier.Fade(false);

        _rearDoor.IsInteractable = true;
        _playerController.SetActive();

        yield return null;
    }
}
