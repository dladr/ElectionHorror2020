using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class TextModifier : MonoBehaviour
{
    [SerializeField] private TextMeshPro _textMeshPro;
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

    void UpdateText(string text)
    {
        _textMeshPro.text = text;
    }

    void UpdateColor(Color color)
    {
        _textMeshPro.color = color;
    }
    [Button]
    void Fade(float speed = -1, bool isFadingIn = true)
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
