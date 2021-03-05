 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Change to force state on play
	// can be turned off OR on

public class SetActiveOnAwake : MonoBehaviour {

	public bool IsActiveOnAwake;

	// Use this for initialization
	void Awake () {
		gameObject.SetActive(IsActiveOnAwake);
	}
	

}
