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

  public void UpdateSpriteRendererAlphas(float alpha)
  {
      foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
      {
          var color = spriteRenderer.color;
          color = new Color(color.r, color.g, color.b, alpha);
          spriteRenderer.color = color;
      }
  }

  public void CallChangeAlphaOverTime(float timeToChange, float alpha)
  {
      StartCoroutine(ChangeAlphaOverTime(timeToChange, alpha));
  }

  IEnumerator ChangeAlphaOverTime(float timeToChange, float alpha)
  {
      float timeElapsed = 0;
      float startingAlpha = _spriteRenderers[0].color.a;

      while (timeElapsed < timeToChange)
      {
          timeElapsed += Time.deltaTime;
          float lerpPercent = timeElapsed / timeToChange;

          UpdateSpriteRendererAlphas(Mathf.Lerp(startingAlpha, alpha, lerpPercent));

          yield return new WaitForEndOfFrame();
      }

      yield return null;
  }

    // Update is called once per frame
    void Update()
    {
        
    }
}
