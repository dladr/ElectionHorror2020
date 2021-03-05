using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Sirenix.OdinInspector;
using UnityEngine;

public class MoveArena : MonoBehaviour
{
    [SerializeField] private Transform _destinationTransform;
    [SerializeField] private Transform _originTransform;
    [SerializeField] private Transform _roadWithGuardrailHole;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public void MoveArenaDestination()
    {
        SingletonManager.Get<PlayerController>().transform.parent = transform;
        transform.position = _destinationTransform.position;
        transform.rotation = _destinationTransform.rotation;
        _roadWithGuardrailHole.localScale = new Vector3(1, 1, 1);
        SingletonManager.Get<PlayerController>().transform.parent = null;
    }

    [Button]
    public void MoveArenaOrigin()
    {
        SingletonManager.Get<PlayerController>().transform.parent = transform;
        transform.position = _originTransform.position;
        transform.rotation = _originTransform.rotation;
        _roadWithGuardrailHole.localScale = new Vector3(-1, 1, 1);
        SingletonManager.Get<PlayerController>().transform.parent = null;
    }
}
