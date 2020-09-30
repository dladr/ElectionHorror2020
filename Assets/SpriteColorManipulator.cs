using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColorManipulator : MonoBehaviour
{
    private SpriteRenderer[] _spriteRenderers;

    [SerializeField] private Color _startingColor;
    // Start is called before the first frame update
    void Awake()
    {
        GetSpriteRenderers();
        UpdateSpriteRendererColors(_startingColor);
    }

    void GetSpriteRenderers()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

  public  void UpdateSpriteRendererColors(Color color)
    {
        foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
