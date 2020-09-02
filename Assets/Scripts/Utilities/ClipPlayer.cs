using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class ClipPlayer : MonoBehaviour {

	public AudioClip[] Clips;
	public float PitchRandomization = .3f;

    [SerializeField]
    private Queue<AudioClip> _clipQueue;

	public bool PlayOnTimer;
	public Vector2 MinMaxPlaytime;
	public bool ClipQueued = false;
	
    [SerializeField]
	private AudioSource _audioSource;
	// Use this for initialization
	void Awake () {
		_clipQueue = new Queue<AudioClip>();
		_audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayOnTimer && !ClipQueued)
		{
			ClipQueued = true;
			var timeToPlay = Random.Range(MinMaxPlaytime.x, MinMaxPlaytime.y);
			Invoke("PlayClip", timeToPlay);
		}
	}	
	
	void LoadClips()
	{
		Clips.ShuffleArray();
		foreach (var clip in Clips)
		{
			_clipQueue.Enqueue(clip);
		}
	}

	public void PlayClip()
	{
		ClipQueued = false;
		if (!_clipQueue.Any())
			LoadClips();
		var clip  = _clipQueue.Dequeue();
		PlayClip(clip);
	
	}

	public void PlayClip(AudioClip clip)
	{
		_audioSource.clip = clip; 

		_audioSource.pitch = 1 + Random.Range(-PitchRandomization, PitchRandomization);
		_audioSource.Play();
	}

	public void PlayClip(int index)
	{
		PlayClip(Clips[index]);
	}

}
