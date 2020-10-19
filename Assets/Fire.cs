using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private Light _light;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private bool IsAutoFire;

    // Start is called before the first frame update
    void Start()
    {
        if(IsAutoFire)
            FireGun();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireGun()
    {
        _light.enabled = true;
        _audioSource.Play();
        Invoke(nameof(TurnOffLight), .05f);
    }

    public void TurnOffLight()
    {
        _light.enabled = false;

        if(IsAutoFire)
            Invoke(nameof(FireGun), Random.Range(.3f, .6f));
    }
}
