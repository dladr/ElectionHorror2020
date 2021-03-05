using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Sirenix.OdinInspector;
using UnityEngine;

public class RitualBox : MonoBehaviour
{
    [SerializeField] private PostalBox _postalBox;

    [SerializeField] private TriggerMessage _triggerMessage;

    public int NumberOfBallots;

    public int NumberofGhosts;

    private int _ghostIndex;

    public Transform BallotsParentTransform;

    public EnvelopePickup[] Envelops;

    public Ghost[] Ghosts;

    public float TimeBetweenGhosts;

    [SerializeField] private Light _light;

    private Color _originalLightColor;

    private bool _isActive;

    public AudioSource[] TCIntroTones;
    public AudioSource ThunderAudioSource;
    public int TCIndex;

    private void Awake()
    {
        TCIntroTones = GetComponents<AudioSource>();
    }

    void Start()
    {
        _postalBox.IsDeactivated = true;

        Envelops = BallotsParentTransform.GetComponentsInChildren<EnvelopePickup>();
        NumberOfBallots = Envelops.Length;

        foreach (EnvelopePickup envelopePickup in Envelops)
        {
            envelopePickup.OnPickUpBallot.AddListener(CountBallot);
        }

        foreach (Ghost ghost in Ghosts)
        {
            ghost.OnDieEvent.AddListener(CountGhostDeaths);
        }

        NumberofGhosts = Ghosts.Length;

        _originalLightColor = _light.color;


    }

    
    [Button]
    public void CountBallot()
    {
        NumberOfBallots -= 1;

        if (TCIndex < 10)
        {
            TCIntroTones[9 - TCIndex].Play();
            TCIndex++;
        }

        if (NumberOfBallots <= 0)
        {
            _light.color = Color.red;
            _isActive = true;
            _postalBox.TurnOffGlow();
           // SpawnNextGhost();
           StartCoroutine(SpawnFirstGhost());

        }
    }

  IEnumerator SpawnFirstGhost()
    {
        yield return new WaitForSeconds(5);
        ThunderAudioSource.Play();
        yield return new WaitForSeconds(1);
        SingletonManager.Get<MusicManager>().PlayTrack(4);
        SpawnNextGhost();
        yield return null;
    }
   

    void SpawnNextGhost()
    {
        if (!_isActive)
            return;

        if (_ghostIndex > Ghosts.Length - 2)
        {
            return;
        }

        Ghosts[_ghostIndex].ToggleIsActive();
     //   Ghosts[_ghostIndex].OnDieEvent.AddListener(CountGhostDeaths);
        _ghostIndex++;

        Invoke(nameof(SpawnNextGhost), TimeBetweenGhosts);
    }

    [Button]
    public void CountGhostDeaths()
    {
        NumberofGhosts -= 1;


        if (NumberofGhosts == 1)
        {
            Ghosts[Ghosts.Length -1].ToggleIsActive();
            ThunderAudioSource.Play();
            //    Ghosts[Ghosts.Length - 1].OnDieEvent.AddListener(CountGhostDeaths);
        }

        if (NumberofGhosts < 1)
        {
            TurnOff();
        }

    }

    public void TurnOff()
    {
        _postalBox.IsDeactivated = false;
        _triggerMessage.IsDeactivated = true;
        _postalBox.TurnOnGlow();
        SingletonManager.Get<MusicManager>().StopPlaying();
    }

    public void Reset()
    {
        _isActive = false;

        StopAllCoroutines();

        _ghostIndex = 0;
        NumberOfBallots = Envelops.Length;
        NumberofGhosts = Ghosts.Length;
        _postalBox.IsDeactivated = true;
        _triggerMessage.IsDeactivated = false;
        _light.color = _originalLightColor;

        foreach (EnvelopePickup envelopePickup in Envelops)
        {
            envelopePickup.Reset();
        }

        foreach (Ghost ghost in Ghosts)
        {
            ghost.Reset();
        }

        TCIndex = 0;

        SingletonManager.Get<MusicManager>().StopPlaying();

    }
}
