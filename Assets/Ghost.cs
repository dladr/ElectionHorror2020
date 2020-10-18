using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private bool _isActive;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;
    [SerializeField] private Animator _paperAnim;
    [SerializeField] private Transform _paperTransform;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private TextModifier _textModifier;
    [SerializeField] private string DyingWords;
    [SerializeField] private string KillingWords;
    [SerializeField] private FontStyles _fontStyles;
    [SerializeField] private Color _fontColor;
    [SerializeField] private SpriteColorManipulator _spriteColorManipulator;
    [SerializeField] private GhostTrigger _ghostTrigger;

    [SerializeField] private Transform _alternateDestination;
    public bool IsUsingAlternateDestination;

    [SerializeField] private int _health;
    private int _currentHealth;
    [SerializeField] private int _damage;

    private Vector3 _initialPosition;

    public UnityEvent DyingEvent;
    private PlayerController _playerController;
    [SerializeField] bool _isStartingActive;

    [SerializeField] private AudioSource _deathAudioSource;


    private static readonly int IsHorizontal = Animator.StringToHash("IsHorizontal");


    void Awake()
    {

        _initialPosition = transform.position;
        _textModifier = SingletonManager.Get<TextModifier>();
        _collider = GetComponent<Collider>();
        _playerController = SingletonManager.Get<PlayerController>();
        _currentHealth = _health;
        _deathAudioSource = GetComponent<AudioSource>();


    }

    private void Start()
    {
        if (!_isStartingActive)
            ToggleIsActive();
    }


    void Update()
    {
        if (!_isActive)
            return;

        TurnTowardsTarget();
        Move(Vector2.zero);
    }

    void Move(Vector2 inputDirection)
    {
        
        _rigidbody.velocity = transform.forward * _speed;

        UpdatePaperAnim(inputDirection);
    }

    void UpdatePaperAnim(Vector2 inputDirection)
    {
        if (Mathf.Abs(inputDirection.x) > Mathf.Abs(inputDirection.y))
            _paperAnim.SetBool(IsHorizontal, true);

        if (Mathf.Abs(inputDirection.x) < Mathf.Abs(inputDirection.y))
            _paperAnim.SetBool(IsHorizontal, false);


        int scaleX = inputDirection.x < 0 ? 1 : -1;
        int scaleZ = inputDirection.y < 0 ? 1 : -1;
        if (inputDirection != Vector2.zero)
            _paperTransform.localScale = new Vector3(scaleX, 1, scaleZ);

    }


    void TurnTowardsTarget()
    {
        if (IsUsingAlternateDestination)
        {
            transform.LookAt(_alternateDestination);
        }

        else
        {
            transform.LookAt(_playerController.transform);
        }

        _paperTransform.LookAt(_playerController.transform);
        _paperTransform.Rotate(0, 180, 0);

    }


    public void Disappear()
    {
        if (!_isActive)
            return;

        if (!DyingWords.IsNullOrWhitespace())
        {
            _textModifier.UpdateTextTrio(DyingWords, _fontColor, _fontStyles);
            _textModifier.AutoTimeFades();
        }

        if(!_deathAudioSource.SafeIsUnityNull())
                _deathAudioSource.Play();
        DyingEvent.Invoke();
        ToggleIsActive();
        
       
        
    }

    public void Reset()
    {
        if(_isStartingActive != _isActive)
            ToggleIsActive();

        transform.position = _initialPosition;

        _currentHealth = _health;

        if(!_ghostTrigger.SafeIsUnityNull())
            _ghostTrigger.HasTriggered = false;


    }

    [Button]
    public void ToggleIsActive()
    {
        _isActive = !_isActive;

        if (!_isActive)
        {
            _spriteColorManipulator.UpdateSpriteRendererAlphas(0);
            _particleSystem.Stop();
            _collider.enabled = false;
            _rigidbody.isKinematic = true;
        }
            
        else
        {
            _spriteColorManipulator.UpdateSpriteRendererColors(_spriteColorManipulator.StartingColor);
            _particleSystem.Play();
            _collider.enabled = true;
            _rigidbody.isKinematic = false;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        if(_currentHealth <= 0)
            Disappear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _textModifier.UpdateTextTrio(KillingWords, _fontColor, _fontStyles);
            _textModifier.AutoTimeFades();
            other.GetComponent<PlayerController>().TakeDamage(_damage);
        }
    }
}
