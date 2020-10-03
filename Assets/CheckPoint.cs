using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class CheckPoint : MonoBehaviour
{
    public Transform PlayerPosition;

    public GameObject PlayerObject;

    public Transform TruckPosition;

    public Ghost[] Ghosts;

    public PostalBox[] PostalBoxes;

    public UnityEvent ResetCheckPoint;

    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        ResetCheckPoint.Invoke();

        PlayerObject.transform.position = PlayerPosition.position;

        if (Ghosts.IsNullOrEmpty())
            Ghosts = GetComponentsInChildren<Ghost>();

        foreach (Ghost ghost in Ghosts)
        {
            ghost.Reset();
        }
    }

    public void SetCheckPoint()
    {
        _gameManager.UpdateLastCheckPoint(this);
    }
}
