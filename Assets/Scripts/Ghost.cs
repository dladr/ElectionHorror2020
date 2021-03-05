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
    [SerializeField] public Animator _paperAnim;
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

    public UnityEvent OnDieEvent;
    private PlayerController _playerController;
    [SerializeField] bool _isStartingActive;

    [SerializeField] private AudioSource _deathAudioSource;

    public bool IsCowardly;
    public float CowardlyRunDistance;
    public float CowardlyWatchDistance;

    public bool IsRunning;
    public bool IsAdvancing;

    public ParticleSystem CowardlyParticleSystem;

    public bool IsBoss;

    [SerializeField] private Color _bossDamageColor;
    [SerializeField] private Color _bossNormalColor;

    private static readonly int IsHorizontal = Animator.StringToHash("IsHorizontal");

    public bool IsEndingGhost;
    public Transform LookTargetTransform;
    public bool IsLookingAtLookTarget;
    public Transform FollowTargetTransform;
    public bool IsPlayerGhost;
    public bool IsPlayerGhostLooking;
    public bool IsLookingUp;
    public bool HasPlayerSetDirection;
    [SerializeField] private float PlayerGhostRotationOffset;

    public bool IsStill;
    private float rotationAngle;

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

        if(IsCowardly)
            CheckIfRunning();

        if (IsPlayerGhost && IsPlayerGhostLooking)
        {
            

          

            if (IsLookingUp)
            {
                transform.eulerAngles = Vector3.up * PlayerGhostRotationOffset;
                return;
            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (!HasPlayerSetDirection)
            {
                if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
                {
                    HasPlayerSetDirection = true;
                }

                else
                {
                    return;
                }
               
            }
               

            if (horizontal > 0)
            {
                rotationAngle = 90 + PlayerGhostRotationOffset;

                if (vertical > 0)
                    rotationAngle = 45 + PlayerGhostRotationOffset;
                if (vertical < 0)
                    rotationAngle = 135 + PlayerGhostRotationOffset;
            }

            if (horizontal < 0)
            {
                rotationAngle = 270 + PlayerGhostRotationOffset;

                if (vertical > 0)
                    rotationAngle = 315 + PlayerGhostRotationOffset;
                if (vertical < 0)
                    rotationAngle = 225 + PlayerGhostRotationOffset;
            }

            if (horizontal == 0)
            {
                if (vertical < 0)
                    rotationAngle = 180 + PlayerGhostRotationOffset;

                if (vertical > 0)
                    rotationAngle = 0 + PlayerGhostRotationOffset;
            }

            transform.eulerAngles = Vector3.up * rotationAngle;

            return;
        }

        if (IsPlayerGhost)
            return;

        if (IsEndingGhost)
            TurnTowardsTargetEndGhost();
        else
        {
            TurnTowardsTarget();
        }
        
        if(!IsStill)
          Move(Vector2.zero);
    }

    private void LateUpdate()
    {
        if (IsPlayerGhost)
            _rigidbody.isKinematic = true;
    }

    void CheckIfRunning()
    {
      

        float distance = Vector3.Distance(transform.position, _playerController.transform.position);

        if (_currentHealth == 0)
        {
            if (distance > 20)
            {
                Disappear();
                return;
            }

            IsRunning = true;
            return;
        }

        if (distance < (CowardlyRunDistance + CowardlyWatchDistance) / 2)
        {
            IsAdvancing = false;
        }

        if (distance < CowardlyWatchDistance)
        {
            IsRunning = true;
        }

        if (distance > CowardlyRunDistance)
        {
            IsRunning = false;
            IsAdvancing = true;
        }


       


    }

    void Move(Vector2 inputDirection)
    {
        float tempSpeed = _speed;

        if (IsCowardly)
        {
            if (IsRunning)
                tempSpeed *= 3;

            if (_currentHealth == 0)
                tempSpeed *= 2;

            if (!IsRunning && !IsAdvancing)
                tempSpeed = 0;
        }
        
        _rigidbody.velocity = transform.forward * tempSpeed;

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

        if (IsCowardly && IsRunning)
        {
            transform.Rotate(0, 180, 0);
        }

        _paperTransform.LookAt(_playerController.transform);

        if(IsCowardly && IsRunning)
            return;
        
        _paperTransform.Rotate(0, 180, 0);


    }

    void TurnTowardsTargetEndGhost()
    {
        if (Vector3.Distance(transform.position, _alternateDestination.position) > .2f)
        {
            transform.LookAt(_alternateDestination);
            IsStill = false;
        }

        else
        {
            IsStill = true;
        }
          


        //if (IsLookingAtLookTarget ||  Vector3.Distance(transform.position, _alternateDestination.position) < .1f)
        //{
        //    _paperTransform.LookAt(LookTargetTransform);
        //}
        //else
        //{
        //    _paperTransform.LookAt(_alternateDestination);
        //}

        _paperTransform.LookAt(LookTargetTransform);

        _paperTransform.Rotate(0, 180, 0);

    }


    public void Disappear()
    {
        if (!_isActive)
            return;

        if (!DyingWords.IsNullOrWhitespace() && !IsCowardly)
        {
            _textModifier.UpdateTextTrio(DyingWords, _fontColor, _fontStyles);
            _textModifier.AutoTimeFades();
        }

        if(!_deathAudioSource.SafeIsUnityNull())
                _deathAudioSource.Play();

        OnDieEvent.Invoke();
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

            if(IsCowardly)
                SingletonManager.Get<OrbManager>().SetCowardlyGhost(this);
        }
    }

    public void SetActive(bool isActive)
    {
        if(isActive != _isActive)
            ToggleIsActive();
    }

    public void TakeDamage(int damageAmount)
    {
        if (IsCowardly)
        {
            return;
        }

        _currentHealth -= damageAmount;
        if(_currentHealth <= 0)
            Disappear();
        else
        {
            if (IsBoss)
            {
                ChangeParticleColor(_bossDamageColor);
                Invoke(nameof(ChangeParticleColorToDefault), .5f);
            }
             
        }
    }

    void ChangeParticleColor(Color color)
    {
        var particleSystemMain = _particleSystem.main;
        particleSystemMain.startColor = color;
    }

    void ChangeParticleColorToDefault()
    {
        ChangeParticleColor(_bossNormalColor);
    }

    public void TakeCowardlyDamage()
    {
        _textModifier.UpdateTextTrio(DyingWords, _fontColor, _fontStyles);
        _textModifier.AutoTimeFades();
        CowardlyParticleSystem.Play();
        _currentHealth = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _textModifier.UpdateTextTrio(KillingWords, _fontColor, _fontStyles);
            _textModifier.AutoTimeFades();
            other.GetComponent<PlayerController>().TakeDamage(_damage);
        }

        if (IsEndingGhost && other.CompareTag("Bag") && other.transform.parent == _alternateDestination)
        {
           PickupBag(other);
        }
    }

  public  void PickupBag(Collider other)
    {
        _paperAnim.Play("GhostMailBag");
        other.transform.parent.gameObject.SetActive(false);
        SetAlternateDestination(FollowTargetTransform);
    }

    public void SetAlternateDestination(Transform destinationTransform)
    {
        _alternateDestination = destinationTransform;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
