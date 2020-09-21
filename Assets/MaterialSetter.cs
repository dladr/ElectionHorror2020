using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSetter : MonoBehaviour
{
    [SerializeField] private Material _material1;

    [SerializeField] private Material _material2;

    private Renderer[] _renderers;
    // Start is called before the first frame update
    void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        SwitchMaterials(_material1, _material2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwitchMaterials(Material OriginalMaterial, Material replacementMaterial)
    {
        foreach (Renderer renderer in _renderers)
        {
            if (renderer.sharedMaterial == OriginalMaterial)
                renderer.material = replacementMaterial;
        }
    }

    public void TurnOffGlow()
    {
        SwitchMaterials(_material2, _material1);
    }
}
