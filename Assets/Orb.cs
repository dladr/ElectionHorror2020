using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    [SerializeField] private Renderer _renderer;

    [SerializeField] private int _damageAmount;

    [SerializeField] private bool _isActive;

    private bool _isAwake;

    [SerializeField] private Light _light;
    // Start is called before the first frame update
    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _light = GetComponentInChildren<Light>();
        _isAwake = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isActive)
            return;

        if (other.CompareTag("Ghost"))
        {
         AttackGhost(other.GetComponent<Ghost>());   
        }
    }

    void AttackGhost(Ghost ghost)
    {
        if (!_isActive)
            return;

        ghost.TakeDamage(_damageAmount);
        _particleSystem.Play();
        Deactivate();
    }

  public  void Deactivate()
  {
      if (!_isAwake)
          return;

        _renderer.enabled = false;
        _light.enabled = false;

        Disarm();
    }

  public void DeactivateWithParticles()
  {
      if (!_isAwake)
          return;

      _particleSystem.Play();
      _renderer.enabled = false;
      _light.enabled = false;

      Disarm();
    }

    public void Reactivate()
    {
        if (!_isAwake)
            return;

        _renderer.enabled = true;
        _light.enabled = true;
    }

    public void Arm()
    {
        _isActive = true;
    }

    public void Disarm()
    {
        _isActive = false;
    }

}
