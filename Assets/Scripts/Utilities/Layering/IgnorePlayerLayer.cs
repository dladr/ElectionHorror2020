using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePlayerLayer : MonoBehaviour
{
	public string PermanentLayer;
	public int PermanentOrder;
	public SpriteRenderer SpriteRenderer;

	// Use this for initialization
	void Start ()
	{
		SpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		SpriteRenderer.sortingLayerName = PermanentLayer;
		SpriteRenderer.sortingOrder = PermanentOrder;
	}
}
