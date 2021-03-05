using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ProximityRenderToggler : MonoBehaviour
{
    private Camera _mainCamera;

    [SerializeField] private Renderer[] _renderers;

    public float DistanceToRender;

    public float distanceToCamera;

    public bool IsHidingRenderers;

    private GameManager _gameManager;
    // Start is called before the first frame update
    void Awake()
    {
        _gameManager = SingletonManager.Get<GameManager>();
        _mainCamera = _gameManager.MainCamera;
        _renderers = GetEnabledRenderers();
        
    }

    private void FixedUpdate()
    {
        ToggleRenders();
    }

    Renderer[] GetEnabledRenderers()
    {
        List<Renderer> AllRenderers = GetComponentsInChildren<Renderer>().ToList();
        List<Renderer> EnabledRenderers = new List<Renderer>();

        foreach (Renderer renderer in AllRenderers)
        {
            if (renderer.enabled)
                EnabledRenderers.Add(renderer);
        }

        return EnabledRenderers.ToArray();

    }

    void ToggleRenders()
    {
        DistanceToRender = _gameManager.RenderDistance;

        distanceToCamera = Vector3.Distance(_mainCamera.transform.position, transform.position);

        if (!IsHidingRenderers && distanceToCamera > DistanceToRender)
        {
            IsHidingRenderers = true;

            foreach (Renderer renderer1 in _renderers)
            {
                renderer1.enabled = false;
            }
        }

        if (IsHidingRenderers && distanceToCamera <= DistanceToRender)
        {
            IsHidingRenderers = false;

            foreach (Renderer renderer1 in _renderers)
            {
                renderer1.enabled = true;
            }
        }
    }

    void ToggleActive()
    {

    }

}
