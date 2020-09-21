using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSetter : MonoBehaviour
{
    [SerializeField] private Material _material1;

    [SerializeField] private Material _material2;

    [SerializeField] private bool _switchMaterialsOnAwake = true;

    [SerializeField] private Color _ghostColor;

    [SerializeField] private bool _isGhost;
    [SerializeField] private string _initialMaterialsReplacementExtension;

   [SerializeField] private Renderer[] _renderers;
    // Start is called before the first frame update
    void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();

        if(_switchMaterialsOnAwake)
           SwitchMaterials(_material1, _material2);

        if(_isGhost)
            SetupGhosts();
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

    void SetupGhosts()
    {
        MaterialPropertyBlock ghostMaterialPropertyBlock = new MaterialPropertyBlock();
        ghostMaterialPropertyBlock.SetColor("_Color", _ghostColor);
        ghostMaterialPropertyBlock.SetColor("_EmissionColor", _ghostColor);

        foreach (Renderer renderer in _renderers)
        {
            renderer.SetPropertyBlock(ghostMaterialPropertyBlock);
            // renderer.material.EnableKeyword("_Emission");
        }
    }

    public void TurnOffGlow()
    {
        SwitchMaterials(_material2, _material1);
    }
}
