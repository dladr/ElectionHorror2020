using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRaiserAnimStyle : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private SpriteColorManipulator _spriteColorManipulator;
    [SerializeField] private Ghost _ghost;
    public string OpeningPoseName;
    public string TransitionPoseName;

    private void Start()
    {
        _anim.Play(OpeningPoseName);
        _spriteColorManipulator.UpdateSpriteRendererAlphas(0);
    }

    public void RaiseGhost()
    {
        StartCoroutine(RaiseGhostSequence());
    }

    IEnumerator RaiseGhostSequence()
    {
        _spriteColorManipulator.CallChangeAlphaOverTime(10, .5f);
        yield return new WaitForSeconds(5);
        _anim.Play(TransitionPoseName);
        yield return new WaitForSeconds(2);
        _ghost.SetActive(true);

        yield return null;
    }
}
