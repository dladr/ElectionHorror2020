using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private bool _isActive;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private OrbManager _orbManager;
    [SerializeField] private Collider _collider;

    [SerializeField] private float _currentYRotation;

    [SerializeField] private Animator _paperAnim;
    [SerializeField] private Transform _paperTransform;
    [SerializeField] private GameObject _dropBag;
    [SerializeField] private Transform _carryBagTransform;
    [SerializeField] private Animator _carryBagAnimator;
    [SerializeField] private Animator _dropBagAnimator;

    public bool HasBag;
    public bool IsBagFull;
    public bool IsUpdatingRotation;

    private GameManager _gameManager;


    private static readonly int IsHorizontal = Animator.StringToHash("IsHorizontal");
    private static readonly int HorizontalInput = Animator.StringToHash("HorizontalInput");
    private static readonly int VerticalInput = Animator.StringToHash("VerticalInput");


    void Awake()
    {
        _orbManager = GetComponentInChildren<OrbManager>();
        _collider = GetComponent<Collider>();
        _gameManager = SingletonManager.Get<GameManager>();
    }

    
    void Update()
    {
        if (!_isActive)
            return;

        if(IsUpdatingRotation)
            ResetRotation();

        if(Input.GetButtonDown("Action"))
            _orbManager.StartAttack();

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        Move(new Vector2(xInput, yInput).normalized);
    }

    void Move(Vector2 inputDirection)
    {
    
        _rigidbody.velocity = transform.forward * inputDirection.y * _speed +
                              transform.right * inputDirection.x * _speed;

        UpdatePaperAnim(inputDirection);
    }

    void UpdatePaperAnim(Vector2 inputDirection)
    {
        if(Mathf.Abs(inputDirection.x) > Mathf.Abs(inputDirection.y))
            _paperAnim.SetBool(IsHorizontal, true);

        if (Mathf.Abs(inputDirection.x) < Mathf.Abs(inputDirection.y))
            _paperAnim.SetBool(IsHorizontal, false);

        _paperAnim.SetFloat(HorizontalInput, Mathf.Abs(inputDirection.x));
        _paperAnim.SetFloat(VerticalInput, Mathf.Abs(inputDirection.y));

       int scaleX = inputDirection.x < 0 ? 1 : -1;
       int scaleZ = inputDirection.y < 0 ? 1 : -1;
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

    [Button]
    public void ResetRotation()
    {
        UpdateRotation();
        transform.eulerAngles = new Vector3(0, _currentYRotation, 0);
    }

    [Button]
    void UpdateRotation()
    {
        if (_gameManager.RotationReferences.Count > 1)
        {
            //_gameManager.RotationReferences.OrderBy(x => Vector3.Distance(x.transform.position, transform.position));
            //float distance1 =
            //    Vector3.Distance(_gameManager.RotationReferences[0].transform.position, transform.position);
            //float distance2 =
            //    Vector3.Distance(_gameManager.RotationReferences[1].transform.position, transform.position);

            float shortestDistance = 999999;
            float secondShortestDistance = 999999;
            int shortestDistanceIndex = 0;
            int secondShortestDistanceIndex = 0;

            for (int i = 0; i < _gameManager.RotationReferences.Count; i++)
            {
                float iDistance = Vector3.Distance(_gameManager.RotationReferences[i].transform.position,
                    transform.position);
                if (iDistance < shortestDistance)
                {
                    secondShortestDistance = shortestDistance;
                    secondShortestDistanceIndex = shortestDistanceIndex;
                    shortestDistance = iDistance;
                    shortestDistanceIndex = i;
                }
            }

            float percentBetween = shortestDistance / (shortestDistance + secondShortestDistance);
            _currentYRotation = Mathf.Lerp(_gameManager.RotationReferences[shortestDistanceIndex].transform.eulerAngles.y,
                _gameManager.RotationReferences[secondShortestDistanceIndex].transform.eulerAngles.y, percentBetween);
        }
    }

    [Button]
    public void ToggleIsActive()
    {
        _isActive = !_isActive;
    }

    public void SetActive()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    public void TakeDamage(int damage = 1)
    {
        if (damage > 0)
        {
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
        _dropBagAnimator.SetBool("IsFull", IsBagFull);
        _dropBagAnimator.SetBool("IsVisible", true);
        _dropBag.GetComponentInChildren<DropBagPickup>().IsFull = IsBagFull;

    }
}
