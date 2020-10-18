using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float _speed;

    public bool IsActive;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private OrbManager _orbManager;
    [SerializeField] private Collider _collider;

    [SerializeField] private float _currentYRotation;
    Quaternion _currentRotation;

    private RotationReference _rotationReference1;
    private RotationReference _rotationReference2;
    private float _distance1;
    private float _distance2;
    private float _totalDistance;

    [SerializeField] private Animator _paperAnim;
    [SerializeField] private Transform _paperTransform;
    [SerializeField] private GameObject _dropBag;
    [SerializeField] private Transform _carryBagTransform;
    [SerializeField] private Transform _aheadTransform;
    [SerializeField] private Animator _carryBagAnimator;
   // [SerializeField] private Animator _dropBagAnimator;
   [SerializeField] private DropBagPickup _dropBagPickup;

    private bool _isHorizontal;

    private bool hasMovedY;

    public bool HasBag;
    public bool IsBagFull;
    public bool IsUpdatingRotation;

    private GameManager _gameManager;


    private static readonly int IsHorizontal = Animator.StringToHash("IsHorizontal");
    private static readonly int HorizontalInput = Animator.StringToHash("HorizontalInput");
    private static readonly int VerticalInput = Animator.StringToHash("VerticalInput");

    [SerializeField] private AudioSource _deathAudioSource;

    void Awake()
    {
        _orbManager = GetComponentInChildren<OrbManager>();
        _collider = GetComponent<Collider>();
        _gameManager = SingletonManager.Get<GameManager>();
    }

    
    void Update()
    {
        if (!IsActive)
            return;

       

        if(Input.GetButtonDown("Action"))
            _orbManager.StartAttack();

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        Move(new Vector2(xInput, yInput).normalized);
    }

    private void LateUpdate()
    {
        if (IsUpdatingRotation && hasMovedY)
            ResetRotation();
    }

    void Move(Vector2 inputDirection)
    {
    
        _rigidbody.velocity = transform.forward * inputDirection.y * _speed +
                              transform.right * inputDirection.x * _speed;

        UpdatePaperAnim(inputDirection);

        if (Mathf.Abs(inputDirection.y) > 0)
            hasMovedY = true;
    }

    void UpdatePaperAnim(Vector2 inputDirection)
    {
        if (Mathf.Abs(inputDirection.x) > Mathf.Abs(inputDirection.y))
            _isHorizontal = true;

        if (Mathf.Abs(inputDirection.x) < Mathf.Abs(inputDirection.y))
            _isHorizontal = false;

        _paperAnim.SetBool("IsHorizontal", _isHorizontal);

        _paperAnim.SetFloat(HorizontalInput, Mathf.Abs(inputDirection.x));
        _paperAnim.SetFloat(VerticalInput, Mathf.Abs(inputDirection.y));

        int scaleX = 1;
        int scaleZ = 1;

        if (_isHorizontal)
        {
         scaleZ =  scaleX = inputDirection.x < 0 ? 1 : -1;
        }

        else
        {
          scaleZ =  scaleX = inputDirection.y < 0 ? 1 : -1;
        }
        
         

        if(inputDirection != Vector2.zero) 
            _paperTransform.localScale = new Vector3(scaleX, 1, scaleZ);
        
    }

  

    public void Disappear()
    {
        _paperTransform.gameObject.SetActive(false);
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
        _orbManager.HideOrbs(true);
    }

    public void Reappear()
    {
        _paperTransform.gameObject.SetActive(true);
        _rigidbody.isKinematic = false;
        _collider.enabled = true;
        _orbManager.HideOrbs(false);
        ToggleIsActive();
    }

    public void ReappearWithouActivating()
    {
        _paperTransform.gameObject.SetActive(true);
        _rigidbody.isKinematic = false;
        _collider.enabled = true;
        _orbManager.HideOrbs(false);
    }

    [Button]
    public void ResetRotation()
    {
        //UpdateRotation();
       // transform.eulerAngles = new Vector3(0, _currentYRotation, 0);
       hasMovedY = false;
       UpdateRotationFromCurrentReferences();
       transform.rotation = _currentRotation;
    }

    public void Reset()
    {
        if(_orbManager._numberUnlocked > 1)
          _dropBag.SetActive(false);
        else
        {
            SingletonManager.Get<DropBagPickup>().Reset();
        }

        HasBag = false;
        IsBagFull = false;
        _paperAnim.SetBool("IsHoldingBag", false);
        _carryBagAnimator.SetBool("IsFull", false);
        _carryBagAnimator.SetBool("IsVisible", false);
    }
    void DetermineRotationReferences()
    {
        _gameManager.RotationReferences = _gameManager.RotationReferences.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).ToList();

        int firstInFrontIndex = -1;
        int firstBehindIndex = -1;

        for (int i = 0; i < _gameManager.RotationReferences.Count; i++)
        {
            if (IsPositionInFront(_gameManager.RotationReferences[i].transform.position))
            {
                if (firstInFrontIndex < 0)
                    firstInFrontIndex = i;
            }

            else
            {
                if (firstBehindIndex < 0)
                    firstBehindIndex = 1;
            }

            if(firstInFrontIndex > -1 && firstBehindIndex > -1)
                break;
        }

        if (firstInFrontIndex < 0 || firstBehindIndex < 0)
        {
            firstInFrontIndex = 0;
            firstBehindIndex = 1;
        }

        _rotationReference1 = _gameManager.RotationReferences[firstInFrontIndex];
        _rotationReference2 = _gameManager.RotationReferences[firstBehindIndex];
        _distance1 = Vector3.Distance(transform.position, _rotationReference1.transform.position);
        _distance2 = Vector3.Distance(transform.position, _rotationReference2.transform.position);
        _totalDistance = _distance1 + _distance2;
    }

    bool IsPositionInFront(Vector3 position)
    {
        return Vector3.Distance(position, transform.position) > Vector3.Distance(position, _aheadTransform.position);
        
    }

    void UpdateRotationFromCurrentReferences()
    {
        hasMovedY = false;

        if(_rotationReference1.SafeIsUnityNull())
            DetermineRotationReferences();

        float distance1 = Vector3.Distance(transform.position, _rotationReference1.transform.position);
        float distance2 = Vector3.Distance(transform.position, _rotationReference2.transform.position);

        if (distance1 > _distance1 && distance2 > _distance2)
        {
            DetermineRotationReferences();
        }

        else
        {
            _distance1 = distance1;
            _distance2 = distance2;
        }

        _totalDistance = _distance1 + _distance2;
        float percentLerp = _distance1 / _totalDistance;

        _currentRotation = Quaternion.Lerp(_rotationReference1.transform.rotation,
            _rotationReference2.transform.rotation, percentLerp);
    }

    [Button]
    void UpdateRotation()
    {
        if (_gameManager.RotationReferences.Count > 1)
        {
            _gameManager.RotationReferences = _gameManager.RotationReferences.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).ToList();
            float distance1 =
                Vector3.Distance(_gameManager.RotationReferences[0].transform.position, transform.position);
            float distance2 =
                Vector3.Distance(_gameManager.RotationReferences[1].transform.position, transform.position);

            //float shortestDistance = 999999;
            //float secondShortestDistance = 999999;
            //int shortestDistanceIndex = 0;
            //int secondShortestDistanceIndex = 0;

            //for (int i = 0; i < _gameManager.RotationReferences.Count; i++)
            //{
            //    float iDistance = Vector3.Distance(_gameManager.RotationReferences[i].transform.position,
            //        transform.position);
            //    if (iDistance < shortestDistance)
            //    {
            //        secondShortestDistance = shortestDistance;
            //        secondShortestDistanceIndex = shortestDistanceIndex;
            //        shortestDistance = iDistance;
            //        shortestDistanceIndex = i;
            //    }
            //}

            //shortestDistanceIndex = 0;
            //secondShortestDistanceIndex = 1;
            //shortestDistance = distance1;
            //secondShortestDistance = distance2;

            //float percentBetween = shortestDistance / (shortestDistance + secondShortestDistance);
            //_currentYRotation = Mathf.Lerp(_gameManager.RotationReferences[shortestDistanceIndex].transform.eulerAngles.y,
            //    _gameManager.RotationReferences[secondShortestDistanceIndex].transform.eulerAngles.y, percentBetween);
            //_currentRotation = Quaternion.Lerp(_gameManager.RotationReferences[shortestDistanceIndex].transform.rotation, _gameManager.RotationReferences[secondShortestDistanceIndex].transform.rotation, percentBetween);

            float percentBetween = distance1 / (distance1 + distance2);
            _currentRotation = Quaternion.Lerp(_gameManager.RotationReferences[0].transform.rotation, _gameManager.RotationReferences[1].transform.rotation, percentBetween);
        }
    }

    [Button]
    public void ToggleIsActive()
    {
        IsActive = !IsActive;
    }

    public void SetActive()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
    [Button]
    public void TakeDamage(int damage = 1)
    {
        if (damage > 0)
        {
            _deathAudioSource.Play();
            ToggleIsActive();
            _gameManager.LastCheckPoint.Reset();
        }
    }

    public void GetBag()
    {
        HasBag = true;
        _paperAnim.SetBool("IsHoldingBag", true);
        _carryBagAnimator.SetBool("IsFull", IsBagFull);
        _carryBagAnimator.SetBool("IsVisible", HasBag);
    }

    public void DepositBag()
    {
        HasBag = false;
        IsBagFull = false;
        _carryBagAnimator.SetBool("IsFull", IsBagFull);
        _carryBagAnimator.SetBool("IsVisible", HasBag);
        _paperAnim.SetBool("IsHoldingBag", false);
       
    }

    public void PickupBag(bool isFull)
    {
        IsBagFull = isFull;

        HasBag = true;
        _paperAnim.SetBool("IsHoldingBag", true);
        _carryBagAnimator.SetBool("IsFull", IsBagFull);
        _carryBagAnimator.SetBool("IsVisible", HasBag);
        _dropBagPickup.SetCanSetVisible(true);
        _dropBag.SetActive(false);
    }

    public void DropBag()
    {
        if (!HasBag)
            return;

        HasBag = false;
        _paperAnim.SetBool("IsHoldingBag", false);
        _carryBagAnimator.SetBool("IsVisible", HasBag);
        _dropBag.transform.position = _carryBagTransform.position;
        _dropBag.transform.rotation = _carryBagTransform.rotation;
        _dropBag.SetActive(true);
        _dropBagPickup.SetIsBagFull(IsBagFull);
       // _dropBagAnimator.SetBool("IsFull", IsBagFull);
       _dropBagPickup.SetVisible(true);
        //_dropBagAnimator.SetBool("IsVisible", true);
        _dropBag.GetComponentInChildren<DropBagPickup>().IsFull = IsBagFull;

    }

    public void ResetAnim()
    {
        _paperAnim.SetBool("IsHorizontal", false);
    }

    public void FillBagExternal()
    {
        IsBagFull = true;
        _carryBagAnimator.SetBool("IsFull", true);
    }
}
