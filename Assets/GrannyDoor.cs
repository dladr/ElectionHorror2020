using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrannyDoor : MonoBehaviour
{
    [SerializeField] private bool _isOpen;

    [SerializeField] private bool _isPlayerPresent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_isPlayerPresent && Input.GetButtonDown("Action"))
            OpenDoor();
    }

    void OpenDoor()
    {
        if (_isOpen)
        {
            Debug.Log("Heading outside!");
        }

        else
        {
            Debug.Log("Door is blocked by ghost!");
        }
    }

    public void SetDoorOpen()
    {
        _isOpen = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _isPlayerPresent = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _isPlayerPresent = false;
    }
}
