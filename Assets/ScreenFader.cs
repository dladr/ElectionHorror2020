using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
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

    public void Fade(float speed = -1, bool isFadingIn = true)
    {
        if (speed < 0)
            speed = standardFade;

        if (isFadingIn)
            _anim.Play("FadeIn");
        else
        {
            _anim.Play("FadeOut");
        }

        _anim.speed = speed;
    }
}
