using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
    [SerializeField] private List<bool> _haveAnswered;

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
        _haveAnswered[questionNumber] = true;
    }

    [Button]
    public void SubmitBallotToGameManager()
    {
        for (int i = 0; i < _haveAnswered.Count; i++)
        {
            if (!_haveAnswered[i])
            {
                _ballotAnswers[i] = RandomBool();
            }
        }

        _gameManager.BallotAnswers = _ballotAnswers;
        OnBallotComplete.Invoke();
    }

    bool RandomBool()
    {
        int randomInt = Random.Range(0, 2);
        if (randomInt == 1)
            return true;

        return false;
    }
}
