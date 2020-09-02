using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class PlayerLayer : MonoBehaviour
{
	private SpriteRenderer _player;

	public bool ApplyToChildren;

	private int _originalSortingOrder = 0;

	public float CenterYOffset = 0;

	public SpriteOrder[] SpriteOrders;

	public int Offset = 101;

	[SerializeField]
	private bool isPlayerAbove;

	public bool IsUsingUniversalOffset = true;

	// Use this for initialization
	void Awake()
	{
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();

		if (ApplyToChildren)
		{
			SpriteOrders = GetComponentsInChildren<SpriteRenderer>()
				.Select(x => new SpriteOrder(x, _player.sortingLayerID)).ToArray();
		}
		else
		{
			SpriteOrders = new[]
			{
				new SpriteOrder(GetComponent<SpriteRenderer>(), _player.sortingLayerID),
			};
		}

		if (!SpriteOrders.Any())
		{
			Destroy(this);
		}
	}

	// Update is called once per frame
	void Update()
	{


		if (!_player.enabled && IsUsingUniversalOffset)
		{
			SpriteOrders.ForEach(x => x.Apply(Convert.ToInt32(Mathf.Abs((transform.position.y + CenterYOffset)) * 100)));
		}

		if (!_player.enabled)
			return;

		isPlayerAbove = _player.transform.position.y > this.transform.position.y + CenterYOffset;

		int offset = isPlayerAbove ? Offset : -Offset;

		if (IsUsingUniversalOffset)
		{
			int verticalDistanceToPlayer =
				Convert.ToInt32(Mathf.Abs(_player.transform.position.y - (transform.position.y + CenterYOffset)) * 100);

			if (isPlayerAbove)
			{
				offset = verticalDistanceToPlayer + _player.sortingOrder + 1;
			}

			else
			{
				offset = -verticalDistanceToPlayer - _player.sortingOrder - 1;
			}
		}

		SpriteOrders.ForEach(x => x.Apply(offset, IsUsingUniversalOffset));

	}

	void Apply(SpriteRenderer sp, int offset)
	{
		if (sp == null)
		{
			return;
		}
		sp.sortingLayerID = _player.sortingLayerID;
		sp.sortingOrder = _originalSortingOrder + offset;
	}

	void OnDrawGizmosSelected()
	{
		// Draw a yellow sphere at the transform's position
		Gizmos.color = Color.blue;



		Gizmos.DrawSphere(transform.position.Modify(y: CenterYOffset), .05f);
	}
}

[Serializable]
public class SpriteOrder
{
	public SpriteRenderer Renderer;

	public int OriginalSortOrder;
	public int OriginalLayerId;
	public int CurrentSortingOrder;
	public SpriteOrder(SpriteRenderer renderer, int sortingLayerId)
	{
		if (renderer == null)
		{
			return;
		}
		Renderer = renderer;
		OriginalSortOrder = renderer.sortingOrder;
		OriginalLayerId = renderer.sortingLayerID;
		renderer.sortingLayerID = sortingLayerId;
	}

	public void Apply(int offset, bool IsUsingUniversal = false)
	{
		if (Renderer == null)
		{
			return;
		}

		//if(!IsUsingUniversal)
		CurrentSortingOrder = Renderer.sortingOrder = OriginalSortOrder + offset;

		//		else
		//		{
		//			CurrentSortingOrder = Renderer.sortingOrder = offset;
		//		}
	}


}
