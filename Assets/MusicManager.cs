using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] AudioClips;

    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayTrack(int trackNumber, float startTime = 0)
    {
        _audioSource.Stop();
        _audioSource.clip = AudioClips[trackNumber];
        _audioSource.time = startTime;
        _audioSource.Play();
    }
}
