using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GhostTrigger : MonoBehaviour
{
    public UnityEvent TriggerGhost;

    public bool IsUsingTrigger;
    // Start is called before the first frame update
    void Start()
    {
        TriggerGhost.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsUsingTrigger && other.CompareTag("Player"))
        {
            TriggerGhost.Invoke();
            TriggerGhost.RemoveAllListeners();
        }
    }
}
