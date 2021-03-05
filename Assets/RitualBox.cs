using System;
using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        _postalBox.IsDeactivated = true;

        Envelops = BallotsParentTransform.GetComponentsInChildren<EnvelopePickup>();
        NumberOfBallots = Envelops.Length;

        foreach (EnvelopePickup envelopePickup in Envelops)
        {
            envelopePickup.OnPickUpBallot.AddListener(CountBallot);
        }

        NumberofGhosts = Ghosts.Length;

        _originalLightColor = _light.color;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountBallot()
    {
        NumberOfBallots -= 1;

        if (NumberOfBallots <= 0)
        {
            _light.color = Color.red;
            SpawnNextGhost();
        }
    }

   
   

    void SpawnNextGhost()
    {
        if (_ghostIndex > Ghosts.Length - 2)
        {
            return;
        }

        Ghosts[_ghostIndex].ToggleIsActive();
        Ghosts[_ghostIndex].OnDieEvent.AddListener(CountGhostDeaths);
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
            Ghosts[Ghosts.Length - 1].OnDieEvent.AddListener(CountGhostDeaths);
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
    }

    public void Reset()
    {
        foreach (EnvelopePickup envelopePickup in Envelops)
        {
            envelopePickup.Reset();
        }

        foreach (Ghost ghost in Ghosts)
        {
            ghost.Reset();
        }

        NumberOfBallots = Envelops.Length;
        NumberofGhosts = Ghosts.Length;
        _postalBox.IsDeactivated = true;
        _triggerMessage.IsDeactivated = false;
        _light.color = _originalLightColor;

    }
}
