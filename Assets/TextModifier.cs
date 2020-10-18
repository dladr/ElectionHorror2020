using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class TextModifier : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private TextMeshProUGUI _textMeshProActual;
    public Image IMAGE;
    [SerializeField] private Animator _anim;

    [SerializeField] private float standardFade;

    public bool Islocked;

    [SerializeField] private AudioSource _audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void UpdateText(string text)
    {
        if(Islocked)
            return;

        _textMeshPro.text = _textMeshProActual.text = text;
    }

  public  void UpdateColor(Color color)
    {
        if (Islocked)
            return;

        _textMeshPro.color = _textMeshProActual.color = color;
        
    }

  public void UpdateFontStyle(FontStyles fontStyle)
  {
      if (Islocked)
          return;

        _textMeshPro.fontStyle = _textMeshProActual.fontStyle = fontStyle;
  }

  public void UpdateTextTrio(string text, Color color, FontStyles fontStyle)
  {
      if (Islocked)
          return;

        UpdateText(text);
      UpdateColor(color);
      UpdateFontStyle(fontStyle);
  }

  public void AutoTimeFades(float fadeTime = -1)
  {
      if (Islocked)
          return;

        if (fadeTime == -1)
          fadeTime = 2.5f;

      Fade();
      StartCoroutine(FadeAfterSeconds(fadeTime));

  }

  IEnumerator FadeAfterSeconds(float seconds)
  {
      if (Islocked)
          yield break;

        yield return new WaitForSeconds(seconds);
      Fade(false);
      yield return null;
  }

    [Button]
   public void Fade(bool isFadingIn = true, float speed = -1 )
    {
        if (Islocked)
            return;

        StopAllCoroutines();

        if (speed < 0)
            speed = standardFade;

        if (isFadingIn)
        {
            _anim.Play("FadeIn");

            float random =
            _audioSource.pitch = 1 + Random.Range(-.2f, .1f);
            _audioSource.Play();
        }
            
        else
        {
            _anim.Play("FadeOut");
        }

        _anim.speed = speed;
    }
    
}
