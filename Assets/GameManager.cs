using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Sirenix.Utilities;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<bool> BallotAnswers;

    public CheckPoint LastCheckPoint;

    public List<RotationReference> RotationReferences;

    public string[] TrueStrings;
    public string[] FalseStrings;

    public GameObject FinalSceneGameObject;
    public GameObject[] FinalGameObjectsToTurnOff;

    private bool _hasStartedEnding;

    [SerializeField] private AudioSource[] _crashAudioSources;

    public Camera MainCamera;
    public float RenderDistance;

    private bool _isCountingDownToReset;

    // Start is called before the first frame update
    void Awake()
    {
       RotationReferences = new List<RotationReference>(); 
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    if(!LastCheckPoint.SafeIsUnityNull())
        //        LastCheckPoint.Reset();
        //}

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.Quit();
        //}
    }

    public void UpdateLastCheckPoint(CheckPoint checkPoint)
    {
        if(!checkPoint.SafeIsUnityNull())
         LastCheckPoint = checkPoint;
    }

    public void AddRotationReference(RotationReference rotationReference)
    {
        if(!RotationReferences.Contains(rotationReference))
            RotationReferences.Add(rotationReference);
    }

    public string GetResultsString(int index)
    {
        if (BallotAnswers[index])
            return TrueStrings[index];
        else
        {
            return FalseStrings[index];
        }
    }

    public void StartEnding()
    {
        if (_hasStartedEnding)
            return;

        _hasStartedEnding = true;

        StopAllCoroutines();

        StartCoroutine(EndingStartSequence());
    }

    IEnumerator EndingStartSequence()
    {
        TruckMovement truckMovement = SingletonManager.Get<TruckMovement>();
        truckMovement.ReleaseALLConstraints();

        yield return new WaitForSeconds(2);

        TruckParenter[] truckParenters = FindObjectsOfType<TruckParenter>();
        foreach (TruckParenter truckParenter in truckParenters)
        {
            truckParenter.ReleaseTruck();
        }

        truckMovement.FinalDeactivate();

        SingletonManager.Get<MusicManager>().StopPlaying();

        foreach (GameObject o in FinalGameObjectsToTurnOff)
        {
            o.SetActive(false);
        }

        foreach (AudioSource crashAudioSource in _crashAudioSources)
        {
            crashAudioSource.Play();
        }

       
        
        yield return new WaitForSeconds(3);

        FinalSceneGameObject.SetActive(true);
        yield return null;
    }

    public void CountDownReset()
    {
        if (_isCountingDownToReset)
            return;

        _isCountingDownToReset = true;

        StartCoroutine(ResetAfterSeconds());
    }

    IEnumerator ResetAfterSeconds()
    {
        

        yield return new WaitForSeconds(5);

        if (!_hasStartedEnding)
        {
            _isCountingDownToReset = false;
            LastCheckPoint.Reset();
        }
          
    }
}
