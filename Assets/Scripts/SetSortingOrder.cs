using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSortingOrder : MonoBehaviour
{
    private Renderer[] _renderers;

    [SerializeField] private int _sortingOrder;
    // Start is called before the first frame update
    void Start()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer1 in _renderers)
        {
            renderer1.sortingOrder = _sortingOrder;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
