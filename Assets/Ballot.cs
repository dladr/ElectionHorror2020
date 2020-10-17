using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ballot : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private GameObject _lastSelectedGameObject;
    [SerializeField] List<bool> _ballotAnswers;
    [SerializeField] private GameManager _gameManager;

    public UnityEvent OnBallotComplete;
    // Start is called before the first frame update
    void Awake()
    {
        _eventSystem.SetSelectedGameObject(_lastSelectedGameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (_eventSystem.currentSelectedGameObject.SafeIsUnityNull())
            _eventSystem.SetSelectedGameObject(_lastSelectedGameObject);

        _lastSelectedGameObject = _eventSystem.currentSelectedGameObject;
    }

    public void UpdateBallotAnswer(string answer)
    {
        int questionNumber = int.Parse(answer[0].ToString());
        bool answerBool = answer[1] == 't' ? true : false;
        _ballotAnswers[questionNumber] = answerBool;
    }

    public void SubmitBallotToGameManager()
    {
        _gameManager.BallotAnswers = _ballotAnswers;
        OnBallotComplete.Invoke();
    }
}
