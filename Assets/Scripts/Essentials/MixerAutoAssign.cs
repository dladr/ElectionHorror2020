using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

public class MixerAutoAssign : MonoBehaviour
{
    // Start is called before the first frame update

    
    void Awake()
    {
       AutoAssignMixers();
    }

    [Button]
    void AutoAssignMixers()
    {
        var masterAudioMixer = GetMasterAudioMix();

        if (masterAudioMixer == null)
            throw new Exception("None of your audio mixers is named Master!");

        AudioMixerGroup masterAudioMixerGroup = masterAudioMixer.FindMatchingGroups("Master").First();

        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.outputAudioMixerGroup == null)
            {
                Debug.LogWarning($"{audioSource.name} had no output audio mixer. Assigning to Master. Hit the button on {gameObject.name} in edit mode to make this permanent :)");
                audioSource.outputAudioMixerGroup = masterAudioMixerGroup;
            }
        }
    }

    public static AudioMixer GetMasterAudioMix()
    {
        AudioMixer[] audioMixers = Resources.LoadAll<AudioMixer>("AudioMixers");

        if (audioMixers.Length < 1)
            throw new Exception("You must have at least 1 audio mixer in an AudioMixers folder in resources!");

        AudioMixer masterAudioMixer = audioMixers.FirstOrDefault(x => x.name == "Master");

        return masterAudioMixer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
