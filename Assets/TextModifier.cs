using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class TextModifier : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private Animator _anim;

    [SerializeField] private float standardFade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void UpdateText(string text)
    {
        _textMeshPro.text = text;
    }

  public  void UpdateColor(Color color)
    {
        _textMeshPro.color = color;
    }

  public void UpdateFontStyle(FontStyles fontStyle)
  {
      _textMeshPro.fontStyle = fontStyle;
  }

  public void UpdateTextTrio(string text, Color color, FontStyles fontStyle)
  {
      UpdateText(text);
      UpdateColor(color);
      UpdateFontStyle(fontStyle);
  }

  public void AutoTimeFades()
  {
      StopAllCoroutines();
      Fade();
      StartCoroutine(FadeAfterSeconds(5));

  }

  IEnumerator FadeAfterSeconds(float seconds)
  {
      yield return new WaitForSeconds(seconds);
      Fade(false);
      yield return null;
  }

    [Button]
   public void Fade(bool isFadingIn = true, float speed = -1 )
    {
        if (speed < 0)
            speed = standardFade;

        if(isFadingIn)
            _anim.Play("FadeIn");
        else
        {
            _anim.Play("FadeOut");
        }

        _anim.speed = speed;
    }
    
}
