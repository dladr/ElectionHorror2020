using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] AudioClips;

    public AudioMixerGroup[] _audioMixerGroups;

    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

   

    public void PlayTrack(int trackNumber, float startTime = 0, bool isLooping = true)
    {
        _audioSource.Stop();
        _audioSource.clip = AudioClips[trackNumber];
        _audioSource.outputAudioMixerGroup = _audioMixerGroups[trackNumber];
        _audioSource.time = startTime;
        _audioSource.Play();
        _audioSource.loop = isLooping;
    }

    public void PlayTrack(int trackNumber)
    {
        PlayTrack(trackNumber, 0, false);
    }
    public void StopPlaying()
    {
        _audioSource.Stop();
    }
}
