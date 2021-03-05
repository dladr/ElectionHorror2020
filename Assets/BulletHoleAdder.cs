using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoleAdder : MonoBehaviour
{
    public Material BulletHoleMaterial;

    public Material OriginalMaterial;

    private MeshRenderer _meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        OriginalMaterial = _meshRenderer.material;
    }

    public void AddBulletHoles()
    {
        _meshRenderer.material = BulletHoleMaterial;
    }

    public void Reset()
    {
        _meshRenderer.material = OriginalMaterial;
    }
}
