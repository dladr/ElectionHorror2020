using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using UnityEngine;
using Random = UnityEngine.Random;

public class FaillingRail : MonoBehaviour
{
    [SerializeField] private Collider _collider;

    [SerializeField] private Rigidbody _rigidbody;

    private Vector3 _originalPosition;

    private Quaternion _originalRotation;

    private RigidbodyConstraints _originalRigidbodyConstraints;

    private bool hasSetConstraints;

    [SerializeField] private AudioSource[] _audioSources;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        _originalPosition = transform.localPosition;
        _originalRotation = transform.localRotation;
        _audioSources = GetComponents<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Truck"))
        {
            _collider.isTrigger = false;
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(Vector3.up * 10, ForceMode.VelocityChange);
            Physics.gravity = Vector3.down * 100;

            
            SingletonManager.Get<TruckMovement>().ReleaseConstraints();


            StartCoroutine(PlayRandomSounds());

            SingletonManager.Get<ScreenFader>().Fade(isFadingIn:false);
        }
    }

    IEnumerator PlayRandomSounds()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.Play();
        }

        SingletonManager.Get<GameManager>().StartEnding();
      
        yield return null;
    }

    public void Reset()
    {
        StopAllCoroutines();
        transform.localPosition = _originalPosition;
        transform.localRotation = _originalRotation;
        _collider.isTrigger = true;
        _rigidbody.isKinematic = true;
        Physics.gravity = Vector3.down * 9.81f;

        SingletonManager.Get<TruckMovement>().ResetConstraints();
        
       
    }

}
